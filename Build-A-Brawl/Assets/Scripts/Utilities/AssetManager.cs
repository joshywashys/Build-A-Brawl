using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class AssetManager
{
	/// <summary>
	/// Loads a single asset from an AssetBundle.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="assetName"></param>
	/// <param name="assetBundleName"></param>
	/// <returns>Loaded asset as type T.</returns>
	public static T LoadAsset<T>(string assetName, string assetBundleName) where T : Object
    {
		AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleName));
		if (bundle == null)
		{
			Debug.LogError($"AssetBundle - {assetBundleName} - could not be loaded.");
			return null;
		}

		T loadedAsset = bundle.LoadAsset<T>(assetName);
		bundle.Unload(false);

		return loadedAsset;
	}

	/// <summary>
	/// Loads all assets from an AssetBundle.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="assetBundleName"></param>
	/// <returns>Loaded assets as a type T array.</returns>
	public static T[] LoadAllAssets<T>(string assetBundleName) where T : Object
    {
		AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleName));
		if (bundle == null)
		{
			Debug.LogError($"AssetBundle - {assetBundleName} - could not be loaded.");
			return null;
		}

		T[] loadedAssets = bundle.LoadAllAssets<T>();
		bundle.Unload(false);

		return loadedAssets;
	}

	/// <summary>
	/// Loaded a single asset from an AssetBundle using a coroutine.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="assetName"></param>
	/// <param name="assetBundleName"></param>
	/// <returns>A Task of type T. The function can be awaited to get the loaded asset.</returns>
	public static async Task<T> LoadAssetAsync<T>(string assetName, string assetBundleName) where T : Object
    {
		AssetBundleCreateRequest bunderRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, assetBundleName));
		await Task.FromResult(bunderRequest);

		AssetBundle bundle = bunderRequest.assetBundle;
		if (bundle == null)
		{
			Debug.LogError($"AssetBundle - {assetBundleName} - could not be loaded.");
			return null;
		}

		T asset = bundle.LoadAsset<T>(assetName);
		bundle.Unload(false);

		return asset;
	}

	/// <summary>
	/// Loaded all assets from an AssetBundle using a coroutine.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="assetBundleName"></param>
	/// <returns>A Task of type T. The function can be awaited to get the loaded assets.</returns>
	public static async Task<T[]> LoadAllAssetsAsync<T>(string assetBundleName) where T :  Object
	{
		AssetBundleCreateRequest bunderRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, assetBundleName));
		await Task.FromResult(bunderRequest);

		AssetBundle bundle = bunderRequest.assetBundle;
		if (bundle == null)
		{
			Debug.LogError($"AssetBundle - {assetBundleName} - could not be loaded.");
			return null;
		}

		T[] assets = bundle.LoadAllAssets<T>();
		bundle.Unload(false);

		return assets;
	}
}
