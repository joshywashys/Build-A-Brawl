using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class CreateAssetBundles
{
	static Dictionary<string, string> assetBundleNicknames;

	[MenuItem("Assets/Build All AssetBundles")]
	static void BuildAllAssetBundles()
	{
		string assetBundleDirectory = "Assets/StreamingAssets";
		AssetDatabase.DeleteAsset(assetBundleDirectory);

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

	static void LoadNicknames()
    {
		assetBundleNicknames = new Dictionary<string, string>();
		foreach (string name in AssetDatabase.GetAllAssetBundleNames())
			assetBundleNicknames.Add(name, name);

		string editorPath = Path.Combine(Application.dataPath, "Editor/BuildAssetBundlePrefs.json");
		if (!File.Exists(editorPath))
		{
			Debug.LogWarning("Failed to load BuildAssetBundlePrefs.json - file not found. Loading defaults...");
			return;
		}

		int assetBundleCount = assetBundleNicknames.Count;
		string[] data = File.ReadAllText(editorPath).Split('\n');
		for (int i = 1; i <= assetBundleCount; i++)
		{
			if (i < assetBundleCount)
				data[i] = data[i].Remove(data[i].Length - 1);

			BuildAssetBundlesPreferencesWindow.AssetBundleNickname nickname = JsonUtility.FromJson<BuildAssetBundlesPreferencesWindow.AssetBundleNickname>(data[i]);
			if (!assetBundleNicknames.ContainsKey(nickname.assetBundleName))
				continue;

			assetBundleNicknames[nickname.assetBundleName] = nickname.assetBundleNickname;
		}
	}

	public static void CreateBundleNameCache()
    {
		LoadNicknames();

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
		string searchPattern = "*.prefab";

		string label = currentDirectory.Replace(Path.Combine(Application.dataPath, "BundledAssets\\"), "");
		foreach(string assetPath in Directory.GetFiles(currentDirectory, searchPattern))
			outAssetPaths.Add(new AssetBundlePathLink(assetPath.Replace(Application.dataPath, "Assets"), label));
		
		outBundleNames.Add(label);

		string[] directories = Directory.GetDirectories(currentDirectory);
		foreach(string directory in directories)
			RecursiveDirectoryAssetScan(directory, ref outAssetPaths, ref outBundleNames);
	}
}

public class BuildAssetBundlesPreferencesWindow : EditorWindow
{
	Dictionary<string, string> assetBundleNicknames;

	GUIStyle headerStyle, subHeaderStyle;

	[MenuItem("Assets/Build AssetBundles Preferences")]
	static void Init()
	{
		BuildAssetBundlesPreferencesWindow prefWindow = (BuildAssetBundlesPreferencesWindow)GetWindow(typeof(BuildAssetBundlesPreferencesWindow));
		prefWindow.titleContent = new GUIContent("Build AssetBundle Preferences");

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

		prefWindow.Refresh();
		prefWindow.Show();
	}

	void Refresh()
	{
		assetBundleNicknames = new Dictionary<string, string>();
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
			assetBundleNicknames.Add(name, nickname);
		}

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

	void ApplySettings()
    {
		string data = "{\"AssetBundle Nicknames\" : [\n";
		
		int currentIndex = 0;
		foreach (KeyValuePair<string, string> nickname in assetBundleNicknames)
			data += JsonUtility.ToJson(new AssetBundleNickname(nickname.Key, nickname.Value)) + ((currentIndex++ == assetBundleNicknames.Count - 1) ? "" : ",") + '\n';
		
		data += "]}";

		string editorPath = Path.Combine(Application.dataPath, "Editor/BuildAssetBundlePrefs.json");
		File.WriteAllText(editorPath, data);

		CreateAssetBundles.CreateBundleNameCache();
		
		AssetDatabase.Refresh();
	}

	void LoadSettings()
    {
		string editorPath = Path.Combine(Application.dataPath, "Editor/BuildAssetBundlePrefs.json");
		if (!File.Exists(editorPath))
		{
			Debug.LogWarning("Failed to load BuildAssetBundlePrefs.json - file not found. Loading defaults...");
			return;
		}

		int assetBundleCount = assetBundleNicknames.Count;
		string[] data = File.ReadAllText(editorPath).Split('\n');
		for (int i = 1; i <= assetBundleCount; i++)
		{
			if (i < assetBundleCount)
				data[i] = data[i].Remove(data[i].Length - 1);

			AssetBundleNickname nickname = JsonUtility.FromJson<AssetBundleNickname>(data[i]);
			if (!assetBundleNicknames.ContainsKey(nickname.assetBundleName))
				continue;

			assetBundleNicknames[nickname.assetBundleName] = nickname.assetBundleNickname;
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

    private void OnValidate()
    {
		Refresh();
    }

    void OnGUI()
	{
		// Header
		GUILayout.Label("Build AssetBundle Preferences", headerStyle);
		GUILayout.Space(25);

		// Nicknames
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

		// Footer Buttons
		GUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Refresh")) Refresh();
			if (GUILayout.Button("Default")) Default();
			if (GUILayout.Button("Apply")) ApplySettings();
		}
		GUILayout.EndHorizontal();
	}
}
