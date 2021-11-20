using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

/*
HOW/WHEN TO USE:
This script handles swapping around parts in the creature creator.
A separate script will be made to export the creature to a playable character.
 */
public class PartCombiner : MonoBehaviour
{
    public Transform creatureContainer;
	public int maxSaveSlots = 8;

	private static bool m_resourcesLoaded = false;
	private static Dictionary<string, GameObject[]> partsList;

    private int headIndex;
    private int TorsoIndex;
    private int ArmLIndex;
    private int ArmRIndex;
    private int LegIndex;

    private GameObject currHead;
	private GameObject currTorso;
	private GameObject currArmL;
    private GameObject currArmR;
    private GameObject currLegs;

	private GameObject newHead;
	private GameObject newTorso;
	private GameObject newArmL;
    private GameObject newArmR;
    private GameObject newLegs;

	private int[] bodyParts;

	private CreatureData creature;
	public CreatureData[] savedCreatureData;

	//we will recreate the creature every time a part is swapped out, despite it not being optimal, since it's not cpu-heavy at all anyways.
	//If it turns out to be cpu-heavy, we can optimize it to adjust part locations rather than re-generate.
	public void generateCreature()
	{
        //should probably do a bunch of checks to make sure each object has it's joints set up properly
        print("button hit");

        //clear previous creature
        Destroy(newHead);
        Destroy(newTorso);
        Destroy(newArmL);
        Destroy(newArmR);
        Destroy(newLegs);

        //spawn parts
        newTorso = Instantiate(currTorso, creatureContainer.position, Quaternion.identity, creatureContainer);
		newHead = Instantiate(currHead, creatureContainer.position, Quaternion.identity, creatureContainer);
		newArmL = Instantiate(currArmL, creatureContainer.position, Quaternion.identity, creatureContainer);
        newArmR = Instantiate(currArmR, creatureContainer.position, Quaternion.identity, creatureContainer);
        newLegs = Instantiate(currLegs, creatureContainer.position, Quaternion.identity, creatureContainer);

        //calculate where to move parts to attach to body parts
        Vector3 headToNeck = newHead.transform.position - newHead.transform.GetChild(0).transform.position;
		Vector3 torsoToNeck = newTorso.transform.position - newTorso.transform.GetChild(0).transform.position;

		Vector3 legsToHips = newLegs.transform.GetChild(0).transform.position - newLegs.transform.position;
		Vector3 torsoToHips = newTorso.transform.position - newTorso.transform.GetChild(3).transform.position;

        Vector3 torsoToShoulderL = newTorso.transform.position + newTorso.transform.GetChild(1).transform.position;
        Vector3 armLToShoulder = newArmL.transform.GetChild(0).transform.position + newArmL.transform.position;

        Vector3 torsoToShoulderR = newTorso.transform.position + newTorso.transform.GetChild(2).transform.position;
        Vector3 armRToShoulder = newArmR.transform.GetChild(0).transform.position + newArmR.transform.position;

        //move each part
        newHead.transform.Translate(headToNeck - torsoToNeck);
		newLegs.transform.Translate(-(legsToHips + torsoToHips));
        newArmL.transform.Translate(torsoToShoulderL - armLToShoulder);
        newArmR.transform.Translate(torsoToShoulderR - armRToShoulder);

        //shift creature upwards
        float headHeight = newHead.GetComponent<Collider>().bounds.size.y;
        float torsoHeight = newTorso.GetComponent<Collider>().bounds.size.y;
        float legsHeight = newLegs.GetComponent<Collider>().bounds.size.y;
        creatureContainer.transform.position = new Vector3(0, (headHeight + torsoHeight + legsHeight)/2, 0);
        //creatureContainer.transform.position = new Vector3(0, , 0);

    }

	// Looking through Unity Documentation highly suggests that the Resources System should not be used out side of Prototyping
	// I'll be keeping this function in the script for the mean time but I'd suggest using Unity's AssetBundle system in for futur development
	// 
	// Using the AssetManager class to load Assetbundles, if you'd like more features get in touch with Tristan
	//
	// https://learn.unity.com/tutorial/assets-resources-and-assetbundles?uv=2017.3#5c7f8528edbc2a002053b5a6
	
	private static void LoadResources()
	{
		GameObject[] headList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Heads");
		GameObject[] torsoList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Torsos");
		GameObject[] armsList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Arms");
		GameObject[] legsList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Legs");

		m_resourcesLoaded = true;
	}
	
	// This will be the function used to handle Loading the game's body part assets
	private static async void LoadAssets()
    {
		partsList = new Dictionary<string, GameObject[]>();
		string[] bundleNames = 
		{
			BundleNameCache.creaturepartsHeads,
			BundleNameCache.creaturepartsTorsos,
			BundleNameCache.creaturepartsArmsL,
            BundleNameCache.creaturepartsArmsR,
            BundleNameCache.creaturepartsLegs
		};

		for (int i = 0; i < bundleNames.Length; i++)
			partsList.Add(bundleNames[i], await AssetManager.LoadAllAssetsAsync<GameObject>(bundleNames[i]));

		m_resourcesLoaded = true;
	}

	void InitializeCreatureGeneration()
	{
		//generate part lists:

		// Load Resources only if they aren't loaded in already.
		if (!m_resourcesLoaded)
			LoadAssets();

		savedCreatureData = new CreatureData[maxSaveSlots];

		// This is for testing... set true to use save - set false to load saved data
#if true
		//set starting part to be a random one and save output
		headIndex = Random.Range(0, partsList[BundleNameCache.creaturepartsHeads].Length);
		TorsoIndex = Random.Range(0, partsList[BundleNameCache.creaturepartsTorsos].Length);
		ArmLIndex = Random.Range(0, partsList[BundleNameCache.creaturepartsArmsL].Length);
        ArmRIndex = Random.Range(0, partsList[BundleNameCache.creaturepartsArmsR].Length);
        LegIndex = Random.Range(0, partsList[BundleNameCache.creaturepartsLegs].Length);


		creature = new CreatureData(headIndex, TorsoIndex, ArmLIndex, ArmRIndex, LegIndex, LegIndex);

		SaveCreatureData(0);
#else
		// Load previously saved random output from file
		LoadCreatureData();

		headIndex = savedCreatureData[0].head;
		TorsoIndex = savedCreatureData[0].torso;
		ArmIndex = savedCreatureData[0].armLeft;
		LegIndex = savedCreatureData[0].legLeft;
#endif

		currHead = partsList[BundleNameCache.creaturepartsHeads][headIndex];
		currTorso = partsList[BundleNameCache.creaturepartsTorsos][TorsoIndex];
		currArmL = partsList[BundleNameCache.creaturepartsArmsL][ArmLIndex];
        currArmR = partsList[BundleNameCache.creaturepartsArmsR][ArmRIndex];
        currLegs = partsList[BundleNameCache.creaturepartsLegs][LegIndex];

		generateCreature();
	}

	void Start()
    {
        headIndex = 0;
        TorsoIndex = 0;
        ArmLIndex = 0;
        ArmRIndex = 0;
        LegIndex = 0;
        InitializeCreatureGeneration();
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

    #region partswapping functions
    public void nextHead()
    {
        if (headIndex < partsList[BundleNameCache.creaturepartsHeads].Length - 1)
        {
            headIndex += 1;
            currHead = partsList[BundleNameCache.creaturepartsHeads][headIndex];
            generateCreature();
        }
        print("LENGTH: " + partsList[BundleNameCache.creaturepartsHeads].Length + " INDEX: " + headIndex);
    }

    public void prevHead()
    {
        if (headIndex > 0)
        {
            headIndex -= 1;
            currHead = partsList[BundleNameCache.creaturepartsHeads][headIndex];
            generateCreature();
        }
      
    }

    public void nextTorso()
    {

    }

    public void prevTorso()
    {

    }

    public void nextArmL()
    {

    }

    public void prevArmL()
    {

    }

    public void nextArmR()
    {

    }

    public void prevArmR()
    {

    }

    public void nextLegs()
    {

    }

    public void prevLegs()
    {

    }
    #endregion

}
