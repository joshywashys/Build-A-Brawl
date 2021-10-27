using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class CreateAssetBundles
{
	[MenuItem("Assets/Build All AssetBundles")]
	static void BuildAllAssetBundles()
	{
		string assetBundleDirectory = "Assets/StreamingAssets";
		AssetDatabase.DeleteAsset(assetBundleDirectory);

		if (!Directory.Exists(Application.streamingAssetsPath))
			Directory.CreateDirectory(assetBundleDirectory);

		List<string> outBundleNames = new List<string>();
		foreach (string directory in Directory.GetDirectories(Path.Combine(Application.dataPath, "BundledAssets")))
			RecursiveDirectoryAssetScan(directory, ref outBundleNames);

		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

		CreateBundleNameCache(outBundleNames);

		AssetDatabase.Refresh();
	}

	static void CreateBundleNameCache(List<string> bundleNames)
    {
		using (StreamWriter outfile =
			new StreamWriter(Path.Combine(Application.dataPath, "BundleNameCache.cs")))
		{
			outfile.WriteLine($"// This file has been auto-generated after last AssetBundle build on {System.DateTime.Now}");
			outfile.WriteLine("public static class BundleNameCache {");
			for (int i = 0; i < bundleNames.Count; i++)
			{
				string[] splitName = bundleNames[i].Split('\\');
				string varName = "";
				for (int j = 0; j < splitName.Length; j++)
				{
					if (j == 0)
						splitName[j] = splitName[j].ToLower();
					varName += splitName[j];
				}
				string bundleName = bundleNames[i].Replace("\\", "\\\\");
				outfile.WriteLine($"\tpublic static string {varName} = \"{bundleName}\";");
			}
			outfile.WriteLine("}");
		}
	}

	static void RecursiveDirectoryAssetScan(string currentDirectory, ref List<string> outBundleNames)
	{
		string searchPattern = "*.prefab";

		string label = currentDirectory.Replace(Path.Combine(Application.dataPath, "BundledAssets\\"), "");
		foreach(string assetPath in Directory.GetFiles(currentDirectory, searchPattern))
		{
			string projectRelativeDirectory = assetPath.Replace(Application.dataPath, "Assets");
			AssetImporter.GetAtPath(projectRelativeDirectory).SetAssetBundleNameAndVariant(label, "");
		}
		outBundleNames.Add(label);

		string[] directories = Directory.GetDirectories(currentDirectory);
		foreach(string directory in directories)
			RecursiveDirectoryAssetScan(directory, ref outBundleNames);
	}
}
