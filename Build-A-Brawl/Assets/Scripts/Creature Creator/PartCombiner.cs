using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/*
HOW/WHEN TO USE:
This script handles swapping around parts in the creature creator.
A separate script will be made to export the creature to a playable character.
 */
public class PartCombiner : MonoBehaviour
{
	public Transform generationLocation;
	public int maxSaveSlots = 8;

	private static bool m_resourcesLoaded = false;
	private static Dictionary<string, GameObject[]> partsList;
	
	private GameObject currHead;
	private GameObject currTorso;
	private GameObject currArms;
	private GameObject currLegs;

	private GameObject newHead;
	private GameObject newTorso;
	private GameObject newArms;
	private GameObject newLegs;

	private float legsHeight;

	private int[] bodyParts;
	private CreatureData creature;

	public CreatureData[] savedCreatureData;

	//we will recreate the creature every time a part is swapped out, despite it not being optimal, since it's not cpu-heavy at all anyways.
	//If it turns out to be cpu-heavy, we can optimize it to adjust the part locations.
	public void generateCreature()
	{
		//should probably do a bunch of checks to make sure each object has it's joints set up properly

		//spawn parts
		newTorso = Instantiate(currTorso, generationLocation.position, Quaternion.identity, generationLocation);
		newHead = Instantiate(currHead, generationLocation.position, Quaternion.identity, generationLocation);
		newLegs = Instantiate(currLegs, generationLocation.position, Quaternion.identity, generationLocation);

		//calculate where to move parts to attach to body parts
		float headToNeck = newHead.transform.position.y - newHead.transform.GetChild(0).transform.position.y;
		float torsoToNeck = newTorso.transform.position.y - newTorso.transform.GetChild(0).transform.position.y;

		float legsToHips = newLegs.transform.GetChild(0).transform.position.y - newLegs.transform.position.y;
		float torsoToHips = newTorso.transform.position.y - newTorso.transform.GetChild(3).transform.position.y;

		//move each part
		newHead.transform.Translate(0, headToNeck - torsoToNeck, 0);
		newLegs.transform.Translate(0, -(legsToHips + torsoToHips), 0);

		//shift creature up so that the bottom of the feet are at the spawnLocation
		//generationLocation.transform.Translate...
	}

	// Looking through Unity Documentation highly suggests that the Resources System should not be used out side of Prototyping
	// I'll be keeping this function in the script for the mean time but I'd suggest using Unity's AssetBundle system in for futur development
	// https://learn.unity.com/tutorial/assets-resources-and-assetbundles?uv=2017.3#5c7f8528edbc2a002053b5a6
	/*
	private static void LoadResources()
	{
		headList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Heads");
		torsoList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Torsos");
		armsList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Arms");
		legsList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Legs");

		m_resourcesLoaded = true;
	}
	*/

	// This will be the function used to handle Loading the game's body part assets
	private static IEnumerator LoadAssets()
    {
		partsList = new Dictionary<string, GameObject[]>();
		string[] bundleNames = 
		{
			BundleNameCache.creaturepartsHeads,
			BundleNameCache.creaturepartsTorsos,
			BundleNameCache.creaturepartsArms,
			BundleNameCache.creaturepartsLegs
		};

		for (int i = 0; i < bundleNames.Length; i++) 
		{
			AssetBundleCreateRequest bunderRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, bundleNames[i]));
			yield return bunderRequest;

			AssetBundle bundle = bunderRequest.assetBundle;
			if (bundle == null)
			{
				Debug.LogError("AssetBundle could not be loaded.");
				yield break;
			}

			GameObject[] loadedAssets = bundle.LoadAllAssets<GameObject>();
			partsList.Add(bundleNames[i], loadedAssets);
			bundle.Unload(false);
		}

		m_resourcesLoaded = true;
	}

	IEnumerator GenerateCreature()
	{
		//generate part lists:
		// Load Resources only if they aren't loaded in already.

		if (!m_resourcesLoaded)
			yield return StartCoroutine(LoadAssets());

		savedCreatureData = new CreatureData[maxSaveSlots];

		//set starting part to be a random one
		int headIndex = 0;
		int TorsoIndex = 0;
		int ArmIndex = 0;
		int LegIndex = 0;

		// This is for testing... set true to use save - set false to load saved data;
#if false
		headIndex = Random.Range(0, partsList[BundleNameCache.creaturepartsHeads].Length);
		TorsoIndex = Random.Range(0, partsList[BundleNameCache.creaturepartsTorsos].Length);
		ArmIndex = Random.Range(0, partsList[BundleNameCache.creaturepartsArms].Length);
		LegIndex = Random.Range(0, partsList[BundleNameCache.creaturepartsLegs].Length);

		creature = new CreatureData(headIndex, TorsoIndex, ArmIndex, ArmIndex, LegIndex, LegIndex);

		SaveCreatureData(0);
#else
		LoadCreatureData();

		headIndex = savedCreatureData[0].head;
		TorsoIndex = savedCreatureData[0].torso;
		ArmIndex = savedCreatureData[0].armLeft;
		LegIndex = savedCreatureData[0].legLeft;
#endif

		currHead = partsList[BundleNameCache.creaturepartsHeads][headIndex];
		currTorso = partsList[BundleNameCache.creaturepartsTorsos][TorsoIndex];
		currArms = partsList[BundleNameCache.creaturepartsArms][ArmIndex];
		currLegs = partsList[BundleNameCache.creaturepartsLegs][LegIndex];

		generateCreature();
	}

	void Start()
    {
		StartCoroutine(GenerateCreature());
    }


	[System.Serializable]
	public struct CreatureData
    {
		public CreatureData(int head, int torso, int armLeft, int armRight, int legLeft, int legRight, string name = "")
        {
			this.name = name;
			this.head = head;
			this.torso = torso;
			this.armLeft = armLeft;
			this.armRight = armRight;
			this.legLeft = legLeft;
			this.legRight = legRight;
		}

		public string name;
		public int head;
		public int torso;
		public int armLeft, armRight;
		public int legLeft, legRight;
    }

	public void SaveCreatureData(int saveIndex)
    {
		savedCreatureData[saveIndex] = creature;

		string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\my games\\Build-A-Brawl";
		if (!Directory.Exists(path))
			Directory.CreateDirectory(path);

		string data = "{\"storage\" : [\n";
		for (int i = 0; i < maxSaveSlots; i++)
		{
			savedCreatureData[i].name = "Save Slot " + i.ToString();
			data += JsonUtility.ToJson(savedCreatureData[i]) + ((i == maxSaveSlots - 1) ? "" : ",") + '\n';
		}
		data += "]}";

		File.WriteAllText(path + "\\storage.json", data);
		print("Creature data saved successfully");
	}

	public void LoadCreatureData()
    {
		string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\my games\\Build-A-Brawl\\storage.json";
		if (!File.Exists(path))
		{
			Debug.LogError("Failed to load save file - file not found.");
			return;
		}

		string[] data = File.ReadAllText(path).Split('\n');
		for (int i = 1; i <= maxSaveSlots; i++)
        {
			if (i < maxSaveSlots)
				data[i] = data[i].Remove(data[i].Length - 1);

			savedCreatureData[i - 1] = JsonUtility.FromJson<CreatureData>(data[i]);
        }
    }
}
