using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public static class AssetManager
{
	static AssetManagerMono instance = null;

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
	/// <param name="callback"></param>
	/// <returns>A coroutine of the loading process. Loaded assets will be returned through a callback function as a type T parameter.</returns>
	public static Coroutine LoadAssetAsync<T>(string assetName, string assetBundleName, UnityAction<T> callback) where T : Object
	{
		if (instance == null)
			new GameObject("AssetManager").AddComponent<AssetManagerMono>();

		return instance.StartCoroutine(_LoadAssetAsync(assetName, assetBundleName, callback));
	}

	static IEnumerator _LoadAssetAsync<T>(string assetName, string assetBundleName, UnityAction<T> callback) where T : Object
    {
		AssetBundleCreateRequest bunderRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, assetBundleName));
		yield return bunderRequest;

		AssetBundle bundle = bunderRequest.assetBundle;
		if (bundle == null)
		{
			Debug.LogError($"AssetBundle - {assetBundleName} - could not be loaded.");
			yield break;
		}

		T asset = bundle.LoadAsset<T>(assetName);
		bundle.Unload(false);

		callback(asset);
	}

	/// <summary>
	/// Loaded all assets from an AssetBundle using a coroutine.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="assetBundleName"></param>
	/// <param name="callback"></param>
	/// <returns>A coroutine of the loading process. Loaded assets will be returned through a callback function as a type T[] parameter.</returns>
	public static Coroutine LoadAllAssetsAsync<T>(string assetBundleName, UnityAction<T[]> callback) where T : Object
    {
		if (instance == null)
			instance = new GameObject("AssetManager").AddComponent<AssetManagerMono>();

		return instance.StartCoroutine(_LoadAllAssetsAsync(assetBundleName, callback));
	}

	static IEnumerator _LoadAllAssetsAsync<T>(string assetBundleName, UnityAction<T[]> callback) where T : Object
	{
		AssetBundleCreateRequest bunderRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, assetBundleName));
		yield return bunderRequest;

		AssetBundle bundle = bunderRequest.assetBundle;
		if (bundle == null)
		{
			Debug.LogError($"AssetBundle - {assetBundleName} - could not be loaded.");
			yield break;
		}

		T[] assets = bundle.LoadAllAssets<T>();
		bundle.Unload(false);

		callback(assets);
	}


	// MONOBEHAVIOR FUNCTIONS FOR RUNTIME EXECUTION OF ASYNC FUNCTIONS
	class AssetManagerMono : MonoBehaviour
    {
        private void Start()
        {
			DontDestroyOnLoad(this);
        }
    }
}
