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
    public GameObject creaturePlayable;
    public GameObject creatureManager;

    public GameObject playerPrefab;

    private int headIndex;
    private int torsoIndex;
    private int armLIndex;
    private int armRIndex;
    private int legIndex;

    // Selected/Generated Parts
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

    // Part Attaching Calculation Variables
    private Vector3 headToNeck;
    private Vector3 torsoToNeck;
    private Vector3 legsToHips;
    private Vector3 torsoToHips;
    private Vector3 torsoToShoulderL;
    private Vector3 armLToShoulder;
    private Vector3 torsoToShoulderR;
    private Vector3 armRToShoulder;
    float heightShift;

    // Saving/Loading
    public int maxSaveSlots = 8;
    private static bool m_resourcesLoaded = false;
    private static Dictionary<string, GameObject[]> partsList;
    private CreatureData creature;
	public CreatureData[] savedCreatureData;

    // Creature Floating
    [SerializeField] private bool floating;
    [SerializeField] private float floatHeight;
    [SerializeField] private float floatPeriod;
    private Vector3 posOffset;

    #region Creature Generation

    //we will recreate the creature every time a part is swapped out, despite it not being optimal, since it's not cpu-heavy at all anyways.
    //If it turns out to be cpu-heavy, we can optimize it to adjust part locations rather than re-generate.
    public void generateCreature()
	{
        //should probably do a bunch of checks to make sure each object has it's joints set up properly

        //clear previous creature
        Destroy(newHead);
        Destroy(newTorso);
        Destroy(newArmL);
        Destroy(newArmR);
        Destroy(newLegs);
        Destroy(creaturePlayable.GetComponent<CreatureStats>());
        //creaturePlayable.transform.position = new Vector3(0, 0, 0);

        //spawn parts
        newHead = Instantiate(currHead, creaturePlayable.transform.position, Quaternion.identity, creaturePlayable.transform);
        newTorso = Instantiate(currTorso, creaturePlayable.transform.position, Quaternion.identity, creaturePlayable.transform);
		newArmL = Instantiate(currArmL, creaturePlayable.transform.position, Quaternion.identity, creaturePlayable.transform);
        newArmR = Instantiate(currArmR, creaturePlayable.transform.position, Quaternion.identity, creaturePlayable.transform);
        newLegs = Instantiate(currLegs, creaturePlayable.transform.position, Quaternion.identity, creaturePlayable.transform);

        //calculate where to move parts to attach to body parts
        headToNeck = newHead.transform.position - newHead.transform.GetChild(0).transform.position;
        torsoToNeck = newTorso.transform.position - newTorso.transform.GetChild(0).transform.position;

        legsToHips = newLegs.transform.GetChild(0).transform.position - newLegs.transform.position;
		torsoToHips = newTorso.transform.position - newTorso.transform.GetChild(3).transform.position;

        torsoToShoulderL = newTorso.transform.position + newTorso.transform.GetChild(1).transform.position;
        armLToShoulder = newArmL.transform.GetChild(0).transform.position + newArmL.transform.position;

        torsoToShoulderR = newTorso.transform.position + newTorso.transform.GetChild(2).transform.position;
        armRToShoulder = newArmR.transform.GetChild(0).transform.position + newArmR.transform.position;

        //move each part
        newHead.transform.Translate(headToNeck - torsoToNeck);
        newArmL.transform.Translate(torsoToShoulderL - armLToShoulder);
        newArmR.transform.Translate(torsoToShoulderR - armRToShoulder);
        newLegs.transform.Translate(-(legsToHips + torsoToHips));

        //shift creature upwards
        //float headHeight = GetPartHeight(newHead);
        //float torsoHeight = GetPartHeight(newTorso); 
        //float legsHeight = GetPartHeight(newLegs);
        //creatureContainer.transform.position = new Vector3(0, (torsoHeight + headHeight + legsHeight)/2 + 1, 0);
        heightShift = (legsToHips.y*2 + torsoToHips.y - torsoToNeck.y + headToNeck.y*2) / 2 + 1.5f; //+ creatureContainer.position.y
        creaturePlayable.transform.localPosition = new Vector3(0, heightShift, 0);
        //print("TOTAL HEIGHT: " + heightShift);
    }

    //adds necessary scripts to turn it into a playable character and sends it to the manager
    public void FinalizeCreature()
    {
        #if false
        GameObject savedCreature = Instantiate(creaturePlayable);
        GameObject savedHead = savedCreature.transform.GetChild(0).gameObject;
        GameObject savedTorso = savedCreature.transform.GetChild(1).gameObject;
        GameObject savedArmL = savedCreature.transform.GetChild(2).gameObject;
        GameObject savedArmR = savedCreature.transform.GetChild(3).gameObject;
        GameObject savedLegs = savedCreature.transform.GetChild(4).gameObject;

        savedHead.transform.parent = savedTorso.transform;
        savedArmL.transform.parent = savedTorso.transform;
        savedArmR.transform.parent = savedTorso.transform;
        savedLegs.transform.parent = savedTorso.transform;
        
        savedCreature.AddComponent<CreatureStats>();
        savedTorso.AddComponent<RigidbodyController>();
        savedTorso.AddComponent<PlayerController>();

        savedCreature.GetComponent<CreatureStats>().attachParts(savedHead, savedTorso, savedArmL, savedArmR, savedLegs);
        creatureManager.GetComponent<CreatureManager>().AddCreature(savedCreature);
        Destroy(savedCreature);

        #else

        GameObject newPlayer = Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity);
        GameObject body = newPlayer.transform.GetChild(2).gameObject;
        RigidbodyController rbc = body.GetComponent<RigidbodyController>();
        PlayerController pc = body.GetComponent<PlayerController>();

        GameObject creature = Instantiate(creaturePlayable, newPlayer.transform.position + new Vector3(0, 3, 0), Quaternion.identity, newPlayer.transform.GetChild(2));
        MakeChildrenPlayerLayer(creature);

        //rearrange part hierarchy
        GameObject savedHead = creature.transform.GetChild(0).gameObject;
        GameObject savedTorso = creature.transform.GetChild(1).gameObject;
        GameObject savedArmL = creature.transform.GetChild(2).gameObject;
        GameObject savedArmR = creature.transform.GetChild(3).gameObject;
        GameObject savedLegs = creature.transform.GetChild(4).gameObject;


        savedHead.transform.parent = savedTorso.transform;
        savedArmL.transform.parent = savedTorso.transform;
        savedArmR.transform.parent = savedTorso.transform;
        savedLegs.transform.parent = savedTorso.transform;

        rbc.floatHeight = legsToHips.y * 2 + torsoToHips.y /*legsToHips.y * 2 + torsoToHips.y*/ ;

        //torsoToShoulderL + armLToShoulder * 2
        pc.anchorLeft.position = new Vector3(torsoToShoulderL.x, savedArmL.transform.position.y, -torsoToShoulderL.x + armLToShoulder.x * 2);
        pc.anchorLeft.position = new Vector3(-torsoToShoulderR.x, savedArmR.transform.position.y, -torsoToShoulderR.x + -armRToShoulder.x * 2);
        //newPlayer.GetComponent<RigidbodyController>().floatHeight = legsToHips.y * 2 + torsoToHips.y;
        //DontDestroyOnLoad(newPlayer); //.transform.root.gameObject
        //creatureManager.GetComponent<CreatureManager>().AddCreature(newPlayer);

        //creature.AddComponent<CreatureStats>();
        //creature.GetComponent<CreatureStats>().attachParts(savedHead, savedTorso, savedArmL, savedArmR, savedLegs);

        creatureManager.GetComponent<CreatureManager>().AddCreature(newPlayer);
        //Destroy(newPlayer);

#endif
    }

#endregion

    public void Float()
    {
        Vector3 tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI / floatPeriod) * floatHeight;

        creatureContainer.position = tempPos + new Vector3(0, floatHeight, 0);
    }

#region Monobehaviour Functions

    public void FixedUpdate()
    {
        if (floating) { Float(); }
    }

    void Start()
    {
        creatureManager = GameObject.Find("CCManager");

        headIndex = 0;
        torsoIndex = 0;
        armLIndex = 0;
        armRIndex = 0;
        legIndex = 0;
        InitializeCreatureGeneration();

        posOffset = transform.position;
    }

#endregion

#region Saving/Loading

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
		torsoIndex = Random.Range(0, partsList[BundleNameCache.creaturepartsTorsos].Length);
		armLIndex = Random.Range(0, partsList[BundleNameCache.creaturepartsArmsL].Length);
        armRIndex = Random.Range(0, partsList[BundleNameCache.creaturepartsArmsR].Length);
        legIndex = Random.Range(0, partsList[BundleNameCache.creaturepartsLegs].Length);


		creature = new CreatureData(headIndex, torsoIndex, armLIndex, armRIndex, legIndex, legIndex);

		SaveCreatureData(0);
#else
		// Load previously saved random output from file
		LoadCreatureData();

		headIndex = savedCreatureData[0].head;
		torsoIndex = savedCreatureData[0].torso;
		ArmIndex = savedCreatureData[0].armLeft;
		legIndex = savedCreatureData[0].legLeft;
#endif

		currHead = partsList[BundleNameCache.creaturepartsHeads][headIndex];
		currTorso = partsList[BundleNameCache.creaturepartsTorsos][torsoIndex];
		currArmL = partsList[BundleNameCache.creaturepartsArmsL][armLIndex];
        currArmR = partsList[BundleNameCache.creaturepartsArmsR][armRIndex];
        currLegs = partsList[BundleNameCache.creaturepartsLegs][legIndex];

		generateCreature();
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
#endregion

#region Partswapping
    public void NextHead()
    {
        if (headIndex < partsList[BundleNameCache.creaturepartsHeads].Length - 1)
        {
            headIndex += 1;
            currHead = partsList[BundleNameCache.creaturepartsHeads][headIndex];
            generateCreature();
        }
    }

    public void PrevHead()
    {
        if (headIndex > 0)
        {
            headIndex -= 1;
            currHead = partsList[BundleNameCache.creaturepartsHeads][headIndex];
            generateCreature();
        }
      
    }

    public void NextTorso()
    {
        if (torsoIndex < partsList[BundleNameCache.creaturepartsTorsos].Length - 1)
        {
            torsoIndex += 1;
            currTorso = partsList[BundleNameCache.creaturepartsTorsos][torsoIndex];
            generateCreature();
        }
    }

    public void PrevTorso()
    {
        if (torsoIndex > 0)
        {
            torsoIndex -= 1;
            currTorso = partsList[BundleNameCache.creaturepartsTorsos][torsoIndex];
            generateCreature();
        }

    }

    public void NextArmL()
    {
        if (armLIndex < partsList[BundleNameCache.creaturepartsArmsL].Length - 1)
        {
            armLIndex += 1;
            currArmL = partsList[BundleNameCache.creaturepartsArmsL][armLIndex];
            generateCreature();
        }
    }

    public void PrevArmL()
    {
        if (armLIndex > 0)
        {
            armLIndex -= 1;
            currArmL = partsList[BundleNameCache.creaturepartsArmsL][armLIndex];
            generateCreature();
        }
    }

    public void NextArmR()
    {
        if (armRIndex < partsList[BundleNameCache.creaturepartsArmsR].Length - 1)
        {
            armRIndex += 1;
            currArmR = partsList[BundleNameCache.creaturepartsArmsR][armRIndex];
            generateCreature();
        }
    }

    public void PrevArmR()
    {
        if (armRIndex > 0)
        {
            armRIndex -= 1;
            currArmR = partsList[BundleNameCache.creaturepartsArmsR][armRIndex];
            generateCreature();
        }
    }

    public void NextLegs()
    {
        if (legIndex < partsList[BundleNameCache.creaturepartsLegs].Length - 1)
        {
            legIndex += 1;
            currLegs = partsList[BundleNameCache.creaturepartsLegs][legIndex];
            generateCreature();
        }
    }

    public void PrevLegs()
    {
        if (legIndex > 0)
        {
            legIndex -= 1;
            currLegs = partsList[BundleNameCache.creaturepartsLegs][legIndex];
            generateCreature();
        }
    }
#endregion

#region Deprecated

    public float GetPartHeight(GameObject toCheck)
    {
        float height = -1.0f;
        if (toCheck.GetComponent<Collider>() != null)
        {
            print("- COLLIDER FOUND -");
            height = toCheck.GetComponent<Collider>().bounds.size.y;
        }
        else if (toCheck.GetComponent<MeshFilter>().mesh != null)
        {
            print("- NO COLLIDER - USING MESH DIMS -");
            height = toCheck.GetComponent<MeshFilter>().mesh.bounds.size.y;
        }
        else
        {
            print("- NO MESH OR COLLIDER FOUND -");
        }

        return height;
    }
    #endregion

#region Misc

    public void MakeChildrenPlayerLayer(GameObject child)
    {
        foreach (Transform trans in child.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = 6;
        }
    }

#endregion
}
