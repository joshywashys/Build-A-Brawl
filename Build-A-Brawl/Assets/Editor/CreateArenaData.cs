using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditorInternal;


public class BuildArenaDataPreferencesWindow : EditorWindow
{
	List<ArenaData> arenaMapList = new List<ArenaData>();
	Dictionary<int, string> buildScenePaths = new Dictionary<int, string>();

	static bool openedFromMenu = false;
	static BuildArenaDataPreferencesWindow prefWindow;
	GUIStyle headerStyle, subHeaderStyle;
	Vector2 scrollPos;

	SerializedObject prefWindowSO;
	ReorderableList arenaListRE;

	[MenuItem("Assets/Arena Data Preferences")]
	static void OpenWidnow()
	{
		openedFromMenu = true;
		Init();
		prefWindow.Show();
	}

	[UnityEditor.Callbacks.DidReloadScripts]
	static void Init()
	{
		if (!HasOpenInstances<BuildArenaDataPreferencesWindow>() && !openedFromMenu)
			return;

		prefWindow = GetWindow<BuildArenaDataPreferencesWindow>("Build Arena Data Preferences");

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

		prefWindow.arenaMapList = new List<ArenaData>();
		prefWindow.buildScenePaths = new Dictionary<int, string>();

		Rect _listRect = new Rect(Vector2.zero, Vector2.one * 500f);
		prefWindow.prefWindowSO = new SerializedObject(prefWindow);
		prefWindow.arenaListRE = new ReorderableList(prefWindow.prefWindowSO, prefWindow.prefWindowSO.FindProperty("arenaMapList"), true, true, true, true);

		prefWindow.arenaListRE.drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, "Arena Map List");
		prefWindow.arenaListRE.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocues) =>
		{
			rect.y += 2f;
			rect.height = EditorGUIUtility.singleLineHeight;

			ArenaData arena = prefWindow.arenaMapList[index];
			GUIContent objectLabel = new GUIContent((arena != null) ? arena.displayName : "Empty");
			EditorGUI.PropertyField(rect, prefWindow.arenaListRE.serializedProperty.GetArrayElementAtIndex(index), objectLabel);
		};
	}

	private class ArenaDataMap
	{
		public ArenaData[] arenaData;
	}

	void ApplySceneDataToBuildList()
	{
		ArenaDataMap arenaDataMap = new ArenaDataMap();
		EditorBuildSettingsScene[] scenesToBuild = EditorBuildSettings.scenes;

		for (int i = 0; i < arenaMapList.Count; i++)
        {
			if (arenaMapList[i].buildIndex < 0)
            {
				
			}
        }

		string data = JsonUtility.ToJson(arenaDataMap);
	}

	void OnGUI()
	{
		// Header
		GUILayout.Label("Build Arena Data Preferences", headerStyle);
		GUILayout.Space(15);
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		{
			// Ignore Assets
			GUILayout.Label("Arena Data List", subHeaderStyle);
			{
				prefWindowSO.Update();
				arenaListRE.DoLayoutList();
				prefWindowSO.ApplyModifiedProperties();
			}
		}
		GUILayout.EndScrollView();

		// Footer Buttons
		GUILayout.Space(20);
		GUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Apply")) ApplySceneDataToBuildList();
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(20);
	}
}
