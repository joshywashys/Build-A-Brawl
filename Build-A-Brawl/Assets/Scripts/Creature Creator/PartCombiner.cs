using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Events;

/*
HOW/WHEN TO USE:
This script handles swapping around parts in the creature creator.
A separate script will be made to export the creature to a playable character.
 */
public class PartCombiner : MonoBehaviour
{
    public int playerNum;
    private bool isReady;
    //public bool spawnButtons = true;

    //Anna

    private PlayerConfiguration playerConfigs;

    // Locations/Prefabs for generation
    public Transform creatureContainer;
    public GameObject creaturePlayable;
    public GameObject creatureManager;
    public GameObject playerPrefab;

    // Selected/Generated Parts
    private int headIndex;
    private int torsoIndex;
    private int armLIndex;
    private int armRIndex;
    private int legIndex;
    
    public GameObject currHead;
	public GameObject currTorso;
	public GameObject currArmL;
    public GameObject currArmR;
    public GameObject currLegs;

    public GameObject newPlayer;
    public GameObject newHead;
	public GameObject newTorso;
	public GameObject newArmL;
    public GameObject newArmR;
    public GameObject newLegs;

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
    public static Dictionary<string, GameObject[]> partsList;
    private CreatureData creature;
	public CreatureData[] savedCreatureData;

    // Creature Floating
    [SerializeField] private bool floating;
    [SerializeField] private float floatHeight;
    [SerializeField] private float floatPeriod;
    private Vector3 posOffset;

    // Events
    public UnityEvent onPartSwap;
    public UnityEvent<bool> onFinalize; //, int
    public UnityEvent<int, GameObject> onCreatureSave;


    #region Creature Generation


    //we will recreate the creature every time a part is swapped out, despite it not being optimal, since it's not cpu-heavy at all anyways.
    //If it turns out to be cpu-heavy, we can optimize it to adjust part locations rather than re-generate.
    public void generateCreature()
	{
        //clear previous creature
        Destroy(newHead);
        Destroy(newTorso);
        Destroy(newArmL);
        Destroy(newArmR);
        Destroy(newLegs);
        Destroy(creaturePlayable.GetComponent<CreatureStats>());

        //spawn parts
        newHead = Instantiate(currHead, creaturePlayable.transform.position, creaturePlayable.transform.rotation, creaturePlayable.transform);
        newTorso = Instantiate(currTorso, creaturePlayable.transform.position, creaturePlayable.transform.rotation, creaturePlayable.transform);
		newArmL = Instantiate(currArmL, creaturePlayable.transform.position, creaturePlayable.transform.rotation, creaturePlayable.transform);
        newArmR = Instantiate(currArmR, creaturePlayable.transform.position, creaturePlayable.transform.rotation, creaturePlayable.transform);
        newLegs = Instantiate(currLegs, creaturePlayable.transform.position, creaturePlayable.transform.rotation, creaturePlayable.transform);

        newHead.name = "head";
        newTorso.name = "torso";
        newArmL.name = "armL";
        newArmR.name = "armR";
        newLegs.name = "legs";

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
        newHead.transform.position += headToNeck - torsoToNeck;
        newArmL.transform.position += torsoToShoulderL - armLToShoulder;
        newArmR.transform.position += torsoToShoulderR - armRToShoulder;
        newLegs.transform.position += -(legsToHips + torsoToHips);
        if (newLegs.GetComponent<LegIKRig>() != null) { newLegs.GetComponent<LegIKRig>().enabled = false; }

        //shift creature upwards
        //float headHeight = GetPartHeight(newHead);
        //float torsoHeight = GetPartHeight(newTorso); 
        //float legsHeight = GetPartHeight(newLegs);
        //creatureContainer.transform.position = new Vector3(0, (torsoHeight + headHeight + legsHeight)/2 + 1, 0);
        heightShift = (legsToHips.y*2 + torsoToHips.y - torsoToNeck.y + headToNeck.y*2) / 2 + 1.5f; //+ creatureContainer.position.y
        creaturePlayable.transform.localPosition = new Vector3(0, heightShift, 0);
        //MakeChildrenPlayerLayer(creaturePlayable.transform);
        //print("TOTAL HEIGHT: " + heightShift);

        onPartSwap?.Invoke();
    }

    public void clearCreature()
    {
        Destroy(newHead);
        Destroy(newTorso);
        Destroy(newArmL);
        Destroy(newArmR);
        Destroy(newLegs);
        Destroy(creaturePlayable.GetComponent<CreatureStats>());
    }

    // Add necessary scripts to turn creature into a playable character, send it to the manager, and start moving around as it
    //var playerConfigs = playerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
    //player.GetComponent<PlayerController>().InitializePlayer(playerConfigs[0]);
    //(Anna) I need to find way of taking that player controller and attaching our player configuration
        
    public void FinalizeCreature()
    {
        if (!isReady)
        {
            // Create references
            newPlayer = Instantiate(playerPrefab, creatureContainer.transform.position, Quaternion.identity);
            newPlayer.name = "Creature";
            GameObject body = newPlayer.transform.Find("Body").gameObject;
            RigidbodyController rbc = body.GetComponent<RigidbodyController>();
            PlayerController pc = body.GetComponent<PlayerController>();
            Rigidbody rb = body.GetComponent<Rigidbody>();


            GameObject creature = Instantiate(creaturePlayable, newPlayer.transform.position + new Vector3(0, 3, 0), Quaternion.identity, newPlayer.transform.GetChild(2));

            //rearrange part hierarchy
            GameObject savedHead = creature.transform.GetChild(0).gameObject;
            GameObject savedTorso = creature.transform.GetChild(1).gameObject;
            GameObject savedArmL = creature.transform.GetChild(2).gameObject;
            GameObject savedArmR = creature.transform.GetChild(3).gameObject;
            GameObject savedLegs = creature.transform.GetChild(4).gameObject;

            // Attach creature stats mothership script
            creature.AddComponent<CreatureStats>();
            CreatureStats stats = creature.GetComponent<CreatureStats>();
            stats.attachParts(savedHead, savedTorso, savedArmL, savedArmR, savedLegs, body);
            stats.initializeCreature();
            stats.SetPlayerNum(playerNum);
            
            savedHead.transform.parent = savedTorso.transform;
            savedArmL.transform.parent = savedTorso.transform;
            savedArmR.transform.parent = savedTorso.transform;
            savedLegs.transform.parent = savedTorso.transform;

            // Set new creature stats
            rbc.floatHeight = legsToHips.y * 2 + torsoToHips.y;
            rbc.m_balanceSpringStrength = stats.GetSpringStrengthLegs();
            rbc.m_balanceSpringDamper = stats.GetSpringDamperLegs(); //newPlayer.transform.position + 
            pc.anchorLeft.localPosition = new Vector3(-0.7f, 0.2f, 1f); //new Vector3(torsoToShoulderL.x, savedArmL.transform.GetChild(0).transform.position.y - 0.3f, armLToShoulder.x * 0.5f); //needs fix
            pc.anchorRight.localPosition = new Vector3(0.7f, 0.2f, 1f); //new Vector3(torsoToShoulderR.x, savedArmR.transform.GetChild(0).transform.position.y - 0.3f, -armRToShoulder.x * 0.5f); //needs fix
            pc.fistLeftRigidbody.mass *= stats.fistMassMultiplierR;
            pc.fistRightRigidbody.mass *= stats.fistMassMultiplierR;
            pc.attackForce = (stats.GetStrengthArmL() + stats.GetStrengthArmL() / 2); //change this later to work for both arms in playercontroller
            pc.playerSpeed = stats.GetMoveSpeed();
            pc.jumpHeight = stats.GetJumpHeight();
            pc.rotateSpeed = stats.GetRotateSpeed();
            rb.mass = stats.GetMass();
                //throwforce in playercontroller

            if (savedLegs.GetComponent<LegIKRig>() != null) { savedLegs.GetComponent<LegIKRig>().enabled = true; }
            if (savedLegs.GetComponent<Collider>() != null) { savedLegs.GetComponent<Collider>().enabled = false; }
            if (savedTorso.GetComponent<Collider>() != null) { savedTorso.GetComponent<Collider>().enabled = false; }

            //foreach (Collider col in savedTorso.GetComponents())
            //deactivate all colliders on torso
            //for (int i = 0; i < savedTorso.GetComponents(typeof(Collider)).Length; i++)

            /*
            Component CopyComponent(Component original, GameObject destination)
            {
                System.Type type = original.GetType();
                Component copy = destination.AddComponent(type);
                // Copied fields can be restricted with BindingFlags
                System.Reflection.FieldInfo[] fields = type.GetFields();
                foreach (System.Reflection.FieldInfo field in fields)
                {
                    field.SetValue(copy, field.GetValue(original));
                }
                return copy;
            }
            */

            /*
            T CopyComponent<T>(T original, GameObject destination) where T : Component
            {
                System.Type type = original.GetType();
                Component copy = destination.AddComponent(type);
                System.Reflection.FieldInfo[] fields = type.GetFields();
                foreach (System.Reflection.FieldInfo field in fields)
                {
                    field.SetValue(copy, field.GetValue(original));
                }
                return copy as T;
            }
            */


            // Copy the necessary colliders
            CapsuleCollider bodyCol = body.GetComponent<CapsuleCollider>();
            bool dontDestroyCapsule = true;

            SphereCollider sc = savedTorso.GetComponent<SphereCollider>();
            if (sc != null)
            {
                SphereCollider SCnew = body.AddComponent<SphereCollider>();
                //SCnew.transform.localScale = savedTorso.transform.localScale;
                SCnew.transform.position = sc.transform.position;
                SCnew.center = sc.center * savedTorso.transform.localScale.x;
                SCnew.radius = sc.radius * savedTorso.transform.localScale.x;
            }

            CapsuleCollider cc = savedTorso.GetComponent<CapsuleCollider>();
            if (savedTorso.GetComponent<CapsuleCollider>() != null)
            {
                Destroy(bodyCol);
                dontDestroyCapsule = false;

                CapsuleCollider CCnew = body.AddComponent<CapsuleCollider>();
                //CCnew.transform.localScale = savedTorso.transform.localScale;
                CCnew.transform.position = cc.transform.position;
                CCnew.center = cc.center * savedTorso.transform.localScale.x;
                CCnew.direction = cc.direction;
                CCnew.height = cc.height * savedTorso.transform.localScale.x;
                CCnew.radius = cc.radius * savedTorso.transform.localScale.x;
            }

            BoxCollider bc = savedTorso.GetComponent<BoxCollider>();
            if (bc != null)
            {
                BoxCollider BCnew = body.AddComponent<BoxCollider>();
                //BCnew.transform.localScale = savedTorso.transform.localScale;
                BCnew.transform.position = bc.transform.position;
                BCnew.center = bc.center * savedTorso.transform.localScale.x;
                BCnew.size = bc.size * savedTorso.transform.localScale.x;
            }

            // Disable colliders after copying them over
            foreach (Collider col in savedTorso.GetComponents(typeof(Collider)))
            {
                col.enabled = false;
            }
            if (dontDestroyCapsule)
            {
                Destroy(bodyCol);
            }
            //Destroy(bodyCol);


            MakeChildrenPlayerLayer(creature.transform);
            
            //Anna
            // initializePlayer(newPlayer, playerNum);
            
            //DontDestroyOnLoad(newPlayer); //.transform.root.gameObject
            //creatureManager.GetComponent<CreatureManager>().RemoveCreature(playerNum);
     //       creatureManager.GetComponent<CreatureManager>().AddCreature(newPlayer, playerNum);
            isReady = true;
            clearCreature();

            onCreatureSave?.Invoke(playerNum, newPlayer);
            onFinalize?.Invoke(true);
        }
        else
        {
            isReady = false;
            creatureManager.GetComponent<CreatureManager>().RemoveCreature(playerNum);
            Destroy(newPlayer);
            generateCreature();

            onFinalize?.Invoke(false);
            
        }
        
    }


    #endregion

    #region Monobehaviour Functions

    public void FixedUpdate()
    {
        if (floating) { Float(); }
    }

    void Start()
    {
        //creatureManager = GameObject.Find("CCManager");
        //creatureManager = GameObject.Find("GameManager");

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
		//print("Creature data saved successfully");
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
        if (!isReady && headIndex < partsList[BundleNameCache.creaturepartsHeads].Length - 1)
        {
            headIndex += 1;
            currHead = partsList[BundleNameCache.creaturepartsHeads][headIndex];
            generateCreature();
        }
        else if (!isReady && headIndex == partsList[BundleNameCache.creaturepartsHeads].Length - 1)
        {
            headIndex = 0;
            currHead = partsList[BundleNameCache.creaturepartsHeads][headIndex];
            generateCreature();
        }
    }

    public void PrevHead()
    {
        if (!isReady && headIndex > 0)
        {
            headIndex -= 1;
            currHead = partsList[BundleNameCache.creaturepartsHeads][headIndex];
            generateCreature();
        }
        else if (!isReady && headIndex == 0)
        {
            headIndex = partsList[BundleNameCache.creaturepartsHeads].Length - 1;
            currHead = partsList[BundleNameCache.creaturepartsHeads][headIndex];
            generateCreature();
        }
    }

    public void NextTorso()
    {
        if (!isReady && torsoIndex < partsList[BundleNameCache.creaturepartsTorsos].Length - 1)
        {
            torsoIndex += 1;
            currTorso = partsList[BundleNameCache.creaturepartsTorsos][torsoIndex];
            generateCreature();
        }
        else if (!isReady && torsoIndex == partsList[BundleNameCache.creaturepartsTorsos].Length - 1)
        {
            torsoIndex = 0;
            currTorso = partsList[BundleNameCache.creaturepartsTorsos][torsoIndex];
            generateCreature();
        }
    }

    public void PrevTorso()
    {
        if (!isReady && torsoIndex > 0)
        {
            torsoIndex -= 1;
            currTorso = partsList[BundleNameCache.creaturepartsTorsos][torsoIndex];
            generateCreature();
        }
        else if (!isReady && torsoIndex == 0)
        {
            torsoIndex = partsList[BundleNameCache.creaturepartsTorsos].Length - 1;
            currTorso = partsList[BundleNameCache.creaturepartsTorsos][torsoIndex];
            generateCreature();
        }
    }

    public void NextArmL()
    {
        if (!isReady && armLIndex < partsList[BundleNameCache.creaturepartsArmsL].Length - 1)
        {
            armLIndex += 1;
            currArmL = partsList[BundleNameCache.creaturepartsArmsL][armLIndex];
            generateCreature();
        }
        else if (!isReady && armLIndex == partsList[BundleNameCache.creaturepartsArmsL].Length - 1)
        {
            armLIndex = 0;
            currArmL = partsList[BundleNameCache.creaturepartsArmsL][armLIndex];
            generateCreature();
        }
    }

    public void PrevArmL()
    {
        if (!isReady && armLIndex > 0)
        {
            armLIndex -= 1;
            currArmL = partsList[BundleNameCache.creaturepartsArmsL][armLIndex];
            generateCreature();
        }
        else if (!isReady && armLIndex == 0)
        {
            armLIndex = partsList[BundleNameCache.creaturepartsArmsL].Length - 1;
            currArmL = partsList[BundleNameCache.creaturepartsArmsL][armLIndex];
            generateCreature();
        }
    }

    public void NextArmR()
    {
        if (!isReady && armRIndex < partsList[BundleNameCache.creaturepartsArmsR].Length - 1)
        {
            armRIndex += 1;
            currArmR = partsList[BundleNameCache.creaturepartsArmsR][armRIndex];
            generateCreature();
        }
        else if (!isReady && armRIndex == partsList[BundleNameCache.creaturepartsArmsR].Length - 1)
        {
            armRIndex = 0;
            currArmR = partsList[BundleNameCache.creaturepartsArmsR][armRIndex];
            generateCreature();
        }
    }

    public void PrevArmR()
    {
        if (!isReady && armRIndex > 0)
        {
            armRIndex -= 1;
            currArmR = partsList[BundleNameCache.creaturepartsArmsR][armRIndex];
            generateCreature();
        }
        else if (!isReady && armRIndex == 0)
        {
            armRIndex = partsList[BundleNameCache.creaturepartsArmsR].Length - 1;
            currArmR = partsList[BundleNameCache.creaturepartsArmsR][armRIndex];
            generateCreature();
        }
    }

    public void NextLegs()
    {
        if (!isReady && legIndex < partsList[BundleNameCache.creaturepartsLegs].Length - 1)
        {
            legIndex += 1;
            currLegs = partsList[BundleNameCache.creaturepartsLegs][legIndex];
            generateCreature();
        }
        else if (!isReady && legIndex == partsList[BundleNameCache.creaturepartsLegs].Length - 1)
        {
            legIndex = 0;
            currLegs = partsList[BundleNameCache.creaturepartsLegs][legIndex];
            generateCreature();
        }
    }

    public void PrevLegs()
    {
        if (!isReady && legIndex > 0)
        {
            legIndex -= 1;
            currLegs = partsList[BundleNameCache.creaturepartsLegs][legIndex];
            generateCreature();
        }
        else if (!isReady && legIndex == 0)
        {
            legIndex = partsList[BundleNameCache.creaturepartsLegs].Length - 1;
            currLegs = partsList[BundleNameCache.creaturepartsLegs][legIndex];
            generateCreature();
        }
    }
#endregion

#region Deprecated

    public float GetPartHeight(GameObject toCheck)
    {
        float height = 0f;
        if (toCheck.GetComponent<Collider>() != null)
        {
            print("- COLLIDER FOUND -");
            height = toCheck.GetComponent<Collider>().bounds.size.y;
        }
        else if (toCheck.GetComponent<MeshRenderer>() != null)
        {
            print("- NO COLLIDER - USING MESH DIMS -");
            height = toCheck.GetComponentInChildren<MeshRenderer>().bounds.size.y;
        }
        else
        {
            print("- NO MESH OR COLLIDER FOUND -");
        }

        return height;
    }
    #endregion

    #region Misc

    public void Float()
    {
        Vector3 tempPos = gameObject.transform.position;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI / floatPeriod) * floatHeight;

        creatureContainer.position = tempPos + new Vector3(0, floatHeight, 0);
    }

    public void MakeChildrenPlayerLayer(Transform child)
    {
        child.gameObject.layer = 3;
        foreach (Transform trans in child)
        {
            MakeChildrenPlayerLayer(trans);
        }
    }

#endregion
}
