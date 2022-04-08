using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEditorInternal;

public class CreateAssetBundles
{
	static Dictionary<string, string> assetBundleNicknames;
	static List<string> assetBundleIgnoreListGUID;

	[MenuItem("Assets/Build All AssetBundles")]
	static void BuildAllAssetBundles()
	{
		LoadPrefs();

		string assetBundleDirectory = "Assets/StreamingAssets";
		Debug.Log(AssetDatabase.DeleteAsset(assetBundleDirectory));
		
		if (!Directory.Exists(Application.streamingAssetsPath))
			Directory.CreateDirectory(assetBundleDirectory);

		List<string> outBundleNames = new List<string>();
		List<AssetBundlePathLink> outLinks = new List<AssetBundlePathLink>();
		foreach (string directory in Directory.GetDirectories(Path.Combine(Application.dataPath, "BundledAssets")))
			RecursiveDirectoryAssetScan(directory, ref outLinks, ref outBundleNames);

		SetAssetBundleNames(outLinks);

		AssetDatabase.RemoveUnusedAssetBundleNames();
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

		CreateBundleNameCache();

		AssetDatabase.Refresh();
	}

	public int callbackOrder { get { return 0; } }
	public void OnPreprocessBuild(BuildReport report)
    {
		Debug.Log("Building AssetBundles");
		BuildAllAssetBundles();
    }

	static void LoadPrefs()
    {
		string editorPath = Path.Combine(Application.dataPath, "Editor/BuildAssetBundlePrefs.json");
		if (!File.Exists(editorPath))
		{
			Debug.LogWarning("Failed to load BuildAssetBundlePrefs.json - file not found. Loading defaults...");
			return;
		}

		string data = File.ReadAllText(editorPath);
		BuildAssetBundlesPreferencesWindow.AssetBundlePreferencesData prefs = JsonUtility.FromJson<BuildAssetBundlesPreferencesWindow.AssetBundlePreferencesData>(data);

		assetBundleNicknames = new Dictionary<string, string>();
		foreach (string name in AssetDatabase.GetAllAssetBundleNames())
			assetBundleNicknames.Add(name, name);

		foreach (BuildAssetBundlesPreferencesWindow.AssetBundleNickname nickname in prefs.assetBundleNicknames)
		{
			if (!assetBundleNicknames.ContainsKey(nickname.assetBundleName))
				continue;

			assetBundleNicknames[nickname.assetBundleName] = nickname.assetBundleNickname;
		}

		assetBundleIgnoreListGUID = new List<string>();
		foreach (string assetPath in prefs.assetBundleIgnorePaths)
        {
			string assetGUID = AssetDatabase.AssetPathToGUID(assetPath);
			if (!string.IsNullOrEmpty(assetGUID) && !assetBundleIgnoreListGUID.Contains(assetGUID))
				assetBundleIgnoreListGUID.Add(assetGUID);
		}
	}

	public static void CreateBundleNameCache()
    {
		LoadPrefs();

		using (StreamWriter outfile =
			new StreamWriter(Path.Combine(Application.dataPath, "BundleNameCache.cs")))
		{
			outfile.WriteLine("public static class BundleNameCache {");
			foreach (KeyValuePair<string, string> nickname in assetBundleNicknames)
			{
				string[] splitName = nickname.Value.Split(new char[] { '\\', '/' });
				string varName = "";
				for (int i = 0; i < splitName.Length; i++)
				{
					if (i > 0)
						splitName[i] = char.ToUpper(splitName[i][0]) + splitName[i].Substring(1);
					varName += splitName[i];
				}
				string bundleName = nickname.Key.Replace("\\", "\\\\");
				outfile.WriteLine($"\tpublic static string {varName} = \"{bundleName}\";");
			}
			outfile.WriteLine("}");
		}
	}

	struct AssetBundlePathLink
	{
		public AssetBundlePathLink(string path, string bundle)
		{
			this.path = path;
			this.bundle = bundle;
		}

		public string path;
		public string bundle;
	}

	static void SetAssetBundleNames(List<AssetBundlePathLink> links)
    {
		foreach (AssetBundlePathLink asset in links)
			AssetImporter.GetAtPath(asset.path).SetAssetBundleNameAndVariant(asset.bundle, "");
	}

	static void RecursiveDirectoryAssetScan(string currentDirectory, ref List<AssetBundlePathLink> outAssetPaths, ref List<string> outBundleNames)
	{
		string path = currentDirectory.Replace(Application.dataPath, "Assets");
		if (assetBundleIgnoreListGUID.Contains(AssetDatabase.AssetPathToGUID(path)))
			return;
		
		string searchPattern = "*.prefab";
		string label = currentDirectory.Replace(Path.Combine(Application.dataPath, "BundledAssets\\"), "");
		
		foreach (string assetPath in Directory.GetFiles(currentDirectory, searchPattern))
		{
			path = assetPath.Replace(Application.dataPath, "Assets");
			if (assetBundleIgnoreListGUID.Contains(AssetDatabase.AssetPathToGUID(path)))
				continue;

			outAssetPaths.Add(new AssetBundlePathLink(path, label));
		}
		outBundleNames.Add(label);

		string[] directories = Directory.GetDirectories(currentDirectory);
		foreach(string directory in directories)
			RecursiveDirectoryAssetScan(directory, ref outAssetPaths, ref outBundleNames);
	}
}

public class BuildAssetBundlesPreferencesWindow : EditorWindow
{
	Dictionary<string, string> assetBundleNicknames = new Dictionary<string, string>();
	public List<Object> assetIgnoreList = new List<Object>();

	static bool openedFromMenu = false;
	static BuildAssetBundlesPreferencesWindow prefWindow;
	GUIStyle headerStyle, subHeaderStyle;
	Vector2 scrollPos;

	SerializedObject prefWindowSO;
	ReorderableList ignoreListRE;

	[UnityEditor.Callbacks.DidReloadScripts]
	static void Init()
	{
		if (!HasOpenInstances<BuildAssetBundlesPreferencesWindow>() && !openedFromMenu)
			return;

		prefWindow = GetWindow<BuildAssetBundlesPreferencesWindow>("Build AssetBundle Preferences");
		
		GUIStyle headerStyle = new GUIStyle();
		headerStyle.fontSize = 20;
		headerStyle.fontStyle = FontStyle.Bold;
		headerStyle.normal.textColor = new Color(0.85f, 0.85f, 0.85f);
		headerStyle.padding = new RectOffset(10, 10, 5, 5);
		prefWindow.headerStyle = headerStyle;

		GUIStyle subHeaderStyle = new GUIStyle(headerStyle);
		subHeaderStyle.fontSize = 16;
		subHeaderStyle.padding = new RectOffset(5, 5, 2, 2);
		prefWindow.subHeaderStyle = subHeaderStyle;

		prefWindow.assetBundleNicknames = new Dictionary<string, string>();
		prefWindow.assetIgnoreList = new List<Object>();

		Rect _listRect = new Rect(Vector2.zero, Vector2.one * 500f);
		prefWindow.prefWindowSO = new SerializedObject(prefWindow);
		prefWindow.ignoreListRE = new ReorderableList(prefWindow.prefWindowSO, prefWindow.prefWindowSO.FindProperty("assetIgnoreList"), true, true, true, true);

		prefWindow.ignoreListRE.drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, "Ignored Assets");
		prefWindow.ignoreListRE.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocues) =>
		{
			rect.y += 2f;
			rect.height = EditorGUIUtility.singleLineHeight;

			Object asset = prefWindow.assetIgnoreList[index];
			GUIContent objectLabel = new GUIContent((asset != null) ? asset.name : "Empty");
			EditorGUI.PropertyField(rect, prefWindow.ignoreListRE.serializedProperty.GetArrayElementAtIndex(index), objectLabel);
		};

		prefWindow.Refresh();
	}

	[MenuItem("Assets/Build AssetBundles Preferences")]
	static void OpenWidnow()
    {
		openedFromMenu = true;
		Init();
		prefWindow.Show();
	}

	void Refresh()
	{
		foreach (string name in AssetDatabase.GetAllAssetBundleNames())
		{
			string[] splitName = name.Split(new char[] { '\\', '/' });
			string nickname = "";
			for (int i = 0; i < splitName.Length; i++)
			{
				if (i > 0)
					splitName[i] = char.ToUpper(splitName[i][0]) + splitName[i].Substring(1);
				nickname += splitName[i];
			}

			if (!assetBundleNicknames.ContainsKey(name))
				assetBundleNicknames.Add(name, nickname);
		}
		
		LoadSettings();
	}

	void RefreshWithClear()
    {
		foreach (string name in AssetDatabase.GetAllAssetBundleNames())
		{
			string[] splitName = name.Split(new char[] { '\\', '/' });
			string nickname = "";
			for (int i = 0; i < splitName.Length; i++)
			{
				if (i > 0)
					splitName[i] = char.ToUpper(splitName[i][0]) + splitName[i].Substring(1);
				nickname += splitName[i];
			}

			if (!assetBundleNicknames.ContainsKey(name))
				assetBundleNicknames.Add(name, nickname);
		}

		List<Object> temp = new List<Object>();
		foreach (Object asset in assetIgnoreList)
        {
			if (asset != null)
				temp.Add(asset);
        }
		assetIgnoreList = temp;
		
		LoadSettings();
	}

	[System.Serializable]
	public struct AssetBundleNickname
    {
		public AssetBundleNickname(string name, string nickname)
        {
			assetBundleName = name;
			assetBundleNickname = nickname;
        }

		public string assetBundleName;
		public string assetBundleNickname;
    }

	public struct AssetBundlePreferencesData
    {
		public AssetBundleNickname[] assetBundleNicknames;
		public string[] assetBundleIgnorePaths;
    }

	void ApplySettings()
    {
		AssetBundlePreferencesData prefs = new AssetBundlePreferencesData();

		List<AssetBundleNickname> nicknames = new List<AssetBundleNickname>();
		foreach (KeyValuePair<string, string> nickname in assetBundleNicknames)
			nicknames.Add(new AssetBundleNickname(nickname.Key, nickname.Value));
		prefs.assetBundleNicknames = nicknames.ToArray();

		List<string> ignores = new List<string>();
		foreach (Object ignore in assetIgnoreList)
		{
			if (ignore != null)
				ignores.Add(AssetDatabase.GetAssetPath(ignore));
		}
		prefs.assetBundleIgnorePaths = ignores.ToArray();

		string data = JsonUtility.ToJson(prefs, true);
		string editorPath = Path.Combine(Application.dataPath, "Editor/BuildAssetBundlePrefs.json");
		File.WriteAllText(editorPath, data);

		CreateAssetBundles.CreateBundleNameCache();
		AssetDatabase.Refresh();
		RefreshWithClear();
	}

	void LoadSettings()
    {
		string editorPath = Path.Combine(Application.dataPath, "Editor/BuildAssetBundlePrefs.json");
		if (!File.Exists(editorPath))
		{
			Debug.LogWarning("Failed to load BuildAssetBundlePrefs.json - file not found. Loading defaults...");
			return;
		}

		string data = File.ReadAllText(editorPath);
		AssetBundlePreferencesData prefs = JsonUtility.FromJson<AssetBundlePreferencesData>(data);
		
		foreach (AssetBundleNickname nickname in prefs.assetBundleNicknames)
			assetBundleNicknames[nickname.assetBundleName] = nickname.assetBundleNickname;
		
		foreach (string ignorePath in prefs.assetBundleIgnorePaths)
        {
			Object asset = AssetDatabase.LoadMainAssetAtPath(ignorePath);
			if (asset != null && !assetIgnoreList.Contains(asset))
				assetIgnoreList.Add(asset);
        }			
	}

	void Default()
    {
		AssetBundleNickname[] tempNicknames = new AssetBundleNickname[assetBundleNicknames.Count];
		int currentIndex = 0;
		foreach (string name in assetBundleNicknames.Keys)
		{
			string[] splitName = name.Split(new char[] { '\\', '/' });
			string nickname = "";
			for (int i = 0; i < splitName.Length; i++)
			{
				if (i > 0)
					splitName[i] = char.ToUpper(splitName[i][0]) + splitName[i].Substring(1);
				nickname += splitName[i];
			}
			tempNicknames[currentIndex] = new AssetBundleNickname(name, nickname);
			currentIndex++;
		}

		for (int i = 0; i < tempNicknames.Length; i++)
			assetBundleNicknames[tempNicknames[i].assetBundleName] = tempNicknames[i].assetBundleNickname;
	}

    void OnGUI()
	{
		// Header
		GUILayout.Label("Build AssetBundle Preferences", headerStyle);
		GUILayout.Space(15);
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		{

			// Nicknames
			GUILayout.Space(10);
			GUILayout.Label("AssetBundle Nicknames", subHeaderStyle);
			{
				AssetBundleNickname[] tempNicknames = new AssetBundleNickname[assetBundleNicknames.Count];
				int currentIndex = 0;
				foreach (KeyValuePair<string, string> nickname in assetBundleNicknames)
				{
					tempNicknames[currentIndex] = new AssetBundleNickname(nickname.Key, nickname.Value);

					GUILayout.BeginHorizontal();
					{
						GUILayout.Label(nickname.Key);
						tempNicknames[currentIndex].assetBundleNickname = GUILayout.TextField(nickname.Value);
					}
					GUILayout.EndHorizontal();
					currentIndex++;
				}

				for (int i = 0; i < tempNicknames.Length; i++)
					assetBundleNicknames[tempNicknames[i].assetBundleName] = tempNicknames[i].assetBundleNickname;
			}

			// Ignore Assets
			GUILayout.Space(25);
			GUILayout.Label("AssetBundle Ignore List", subHeaderStyle);
			{
				prefWindowSO.Update();
				ignoreListRE.DoLayoutList();
				prefWindowSO.ApplyModifiedProperties();
			}
		}
		GUILayout.EndScrollView();

		// Footer Buttons
		GUILayout.Space(20);
		GUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Refresh")) RefreshWithClear();
			if (GUILayout.Button("Default")) Default();
			if (GUILayout.Button("Apply")) ApplySettings();
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(20);
	}
}
