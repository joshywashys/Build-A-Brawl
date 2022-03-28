using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//HOW TO USE/PURPOSE
//During runtime, this is the mothership data script. All of a creature's data will be imported here from it's parts so we don't have to access each individual part.
public class CreatureStats : MonoBehaviour
{
    // part references
    private GameObject creature;
    public GameObject ctrlsPart;
    public int playerNum;

    private GameObject head;
    private GameObject torso;
    private GameObject armL;
    private GameObject armR;
    private GameObject legs;

    private BodyPart headPart;
    private BodyPart torsoPart;
    private BodyPart armLPart;
    private BodyPart armRPart;
    private BodyPart legsPart;

    // <0 health means a part will be detached, for heads+torsos creatures die instead
    [Header("Part Health Pools")]
    [SerializeField] private float healthHeadMax = 10;
    [SerializeField] private float healthTorsoMax = 10;
    [SerializeField] private float healthArmLMax = 10;
    [SerializeField] private float healthArmRMax = 10;
    [SerializeField] private float healthLegsMax = 10;

    [SerializeField] private float healthHead = 10;
    [SerializeField] private float healthTorso = 10;
    [SerializeField] private float healthArmL = 10;
    [SerializeField] private float healthArmR = 10;
    [SerializeField] private float healthLegs = 10;

    // Creature Stats
    [Header("Creature Stats")]
    [SerializeField] private float mass = 1;
    [SerializeField] private float jumpHeight = 1; //total mass + leg strength
    [SerializeField] private float moveSpeed = 1; //total mass + leg speed
    [SerializeField] private float rotateSpeed = 1; //strength of all parts + mass
    [SerializeField] private float springStrengthLegs = 1;
    [SerializeField] private float springDamperLegs = 1;

    // Arm Stats
    [Header("Arm Stats")]
    [SerializeField] private float strengthArmL = 10;
    [SerializeField] private float strengthArmR = 10;
    [SerializeField] private float strengthArms = 20;
    [SerializeField] private float springConstantArmL = 80;
    [SerializeField] private float springConstantArmR = 80;
    [SerializeField] private BodyPartData.animType attackTypeL = 0; //0 uses the default system, others would be robot sticks out arms, druid flails/spins around
    [SerializeField] private BodyPartData.animType attackTypeR = 0;
    [SerializeField] private bool canGrabL = true;
    [SerializeField] private bool canGrabR = true;

    // Constants that we can edit to make our Scriptable Objects values nicer
    public const float HEALTH_BASE = 10;
    public const float ARM_STRENGTH_BASE = 120;
    public const float MOVE_SPEED_BASE = 80;
    public const float ROTATE_SPEED_BASE = 10;
    public const float JUMP_HEIGHT_BASE = 150;
    public const float SPRING_CONSTANT_BASE = 80;
    public const float LEG_STRENGTH_BASE = 300;
    public const float LEG_DAMPER_BASE = 500;

    public const float MASS_HEAD_BASE = 1;
    public const float MASS_TORSO_BASE = 3;
    public const float MASS_ARMS_BASE = 1;
    public const float MASS_LEGS_BASE = 1;

    public UnityEvent<int> onDamage;
    public UnityEvent<int> onDeath;

    #region  Internal Functions

    //call when a creature's stats need to be updated (when a limb is knocked off)
    private void recalculate()
    {
        // Mass
        float headMassNew, torsoMassNew, armLMassNew, armRMassNew, legsMassNew;
        headMassNew = torsoMassNew = armLMassNew = armRMassNew = legsMassNew = 1;

        if (head != null) { headMassNew = headPart.getMass(); }
        if (torso != null) { torsoMassNew = torsoPart.getMass(); }
        if (armL != null) { armLMassNew = armLPart.getMass(); }
        if (armR != null) { armRMassNew = armRPart.getMass(); }
        if (legs != null) { legsMassNew = legsPart.getMass(); }

        mass = headMassNew + torsoMassNew + armLMassNew + armRMassNew + legsMassNew;

        jumpHeight = legsPart.getStrengthMultiplier() * JUMP_HEIGHT_BASE / mass;
        moveSpeed = legsPart.getStrengthMultiplier() * MOVE_SPEED_BASE / mass;
        rotateSpeed = legsPart.getStrengthMultiplier() * ROTATE_SPEED_BASE / mass;

    }

    //get parts stats and load them onto this script
    public void initializeCreature()
    {
        // Set refs
        creature = gameObject;
        headPart = head.GetComponent<BodyPart>();
        torsoPart = torso.GetComponent<BodyPart>();
        armLPart = armL.GetComponent<BodyPart>();
        armRPart = armR.GetComponent<BodyPart>();
        legsPart = legs.GetComponent<BodyPart>();

        head.GetComponent<BodyPart>().creature = gameObject.GetComponent<CreatureStats>();
        torso.GetComponent<BodyPart>().creature = gameObject.GetComponent<CreatureStats>();
        armL.GetComponent<BodyPart>().creature = gameObject.GetComponent<CreatureStats>();
        armR.GetComponent<BodyPart>().creature = gameObject.GetComponent<CreatureStats>();
        legs.GetComponent<BodyPart>().creature = gameObject.GetComponent<CreatureStats>();

        // Health pools
        healthHead = healthHeadMax = headPart.getHealthMultiplier() * HEALTH_BASE;
        healthTorso = healthTorsoMax = torsoPart.getHealthMultiplier() * HEALTH_BASE;
        healthArmL = healthArmLMax = armLPart.getHealthMultiplier() * HEALTH_BASE;
        healthArmR = healthArmRMax = armRPart.getHealthMultiplier() * HEALTH_BASE;
        healthLegs = healthLegsMax = legsPart.getHealthMultiplier() * HEALTH_BASE;

        // Creature Stats
        mass = headPart.getMass() * MASS_HEAD_BASE + torsoPart.getMass() * MASS_TORSO_BASE + armLPart.getMass() * MASS_ARMS_BASE + armRPart.getMass() * MASS_ARMS_BASE + legsPart.getMass() * MASS_LEGS_BASE;
        jumpHeight = Mathf.Sqrt(legsPart.getStrengthMultiplier() * JUMP_HEIGHT_BASE / mass) * 2;
        moveSpeed = MOVE_SPEED_BASE / mass;
        rotateSpeed = ROTATE_SPEED_BASE / mass;
        springStrengthLegs = LEG_STRENGTH_BASE * legsPart.getStrengthMultiplier() / mass;
        springDamperLegs = LEG_DAMPER_BASE * legsPart.getStrengthMultiplier() / mass;

        // Arm Stats
        strengthArmL = armLPart.getStrengthMultiplier() * ARM_STRENGTH_BASE;
        strengthArmR = armRPart.getStrengthMultiplier() * ARM_STRENGTH_BASE;
        strengthArms = strengthArmL + strengthArmR;
        springConstantArmL = armLPart.getSpringConstantMultiplier() * SPRING_CONSTANT_BASE;
        springConstantArmR = armRPart.getSpringConstantMultiplier() * SPRING_CONSTANT_BASE;
        attackTypeL = armLPart.getAttackTypeL();
        attackTypeR = armRPart.getAttackTypeR();

        // Toggle Kinematics
        torsoPart.ToggleKinematics(torsoPart.gameObject.transform, true);

    }

    //initialize. called by the creature creator for initial setup.
    public void attachParts(GameObject newHead, GameObject newTorso, GameObject newArmL, GameObject newArmR, GameObject newLegs, GameObject ctrlsObj)
    {
        head = newHead;
        torso = newTorso;
        armL = newArmL;
        armR = newArmR;
        legs = newLegs;
        ctrlsPart = ctrlsObj;
    }

    public void Damage(BodyPartData bodyPart, float incForce)
    {
        float dmg = incForce;
        dmg = 1.0f; //TEMP FOR DEBUGGING

        //headPart = head.GetComponent<BodyPart>(); //head is null. probably has to do with AttachParts being called in PartCombiner
        //print(headPart); // why is it null only in game???
        if (bodyPart == headPart.partData)
        {
            healthHead -= dmg;
            print("head: " + healthHead);
            if (healthHead <= 0)
            {
                detachHead();
            }
        }
        if (bodyPart == torsoPart.partData)
        {
            healthTorso -= dmg;
            print("torso: " + healthTorso);
            if (healthTorso <= 0)
            {
                detachTorso();
            }
        }
        if (bodyPart == armLPart.partData)
        {
            healthArmL -= dmg;
            print("armL: " + healthArmL);
            if (healthArmL <= 0)
            {
                detachArmL();
            }
        }
        if (bodyPart == armRPart.partData)
        {
            healthArmR -= dmg;
            print("armR: " + healthArmR);
            if (healthArmR <= 0)
            {
                detachArmR();
            }
        }
        if (bodyPart == legsPart.partData)
        {
            print("legs damage");
        }
        recalculate();
        
        onDamage?.Invoke(playerNum); //it said to use this might look into later
    }

    public void Kill()
    {
        healthHead = 0;
        healthTorso = 0;
        healthArmL = 0;
        healthArmR = 0;
        healthLegs = 0;

        Destroy(creature.transform.root.gameObject);
        onDamage?.Invoke(playerNum);
        onDeath?.Invoke(playerNum);
    }

    #endregion

    #region MonoBehaviour Functions

    void Start()
    {
        creature = gameObject;
        if (head == null) { head = creature.transform.GetChild(0).GetChild(5).gameObject; }
        if (torso == null) { torso = creature.transform.GetChild(0).gameObject; }
        if (armL == null) { armL = creature.transform.GetChild(0).GetChild(6).gameObject; }
        if (armR == null) { armR = creature.transform.GetChild(0).GetChild(7).gameObject; } print("armR: " + armR);
        if (legs == null) { legs = creature.transform.GetChild(0).GetChild(8).gameObject; } print("legs: " + legs);

        if (headPart == null) { headPart = head.GetComponent<BodyPart>(); }
        if (torsoPart == null) { torsoPart = torso.GetComponent<BodyPart>(); }
        if (armLPart == null) { armLPart = armL.GetComponent<BodyPart>(); }
        if (armRPart == null) { armRPart = armR.GetComponent<BodyPart>(); }
        if (legsPart == null) { legsPart = legs.GetComponent<BodyPart>(); }
    }

    void Awake()
    {

    }

    #endregion

    #region Part Detaching

    private void detachHead() //add throwable obj scripts?
    {
        headPart.ToggleKinematics(headPart.gameObject.transform, false);
        head.transform.parent = null;
        head = null;
        headPart.creature = null;
        if (torso != null) { detachTorso(); } //AFTER head becomes null. prevents infinite loops of head->torso->head->torso...

        recalculate();
        Kill();
    }

    private void detachTorso()
    {
        if (armL != null) { detachArmL(); }
        if (armR != null) { detachArmR(); }
        if (legs != null) { detachLegs(); }
        if (head != null) { detachHead(); }

        torsoPart.ToggleKinematics(torsoPart.gameObject.transform, false);
        torso.transform.parent = null;
        torso = null;
        torsoPart.creature = null;

        recalculate();
        Kill();
    }

    private void detachArmL()
    {
        armLPart.ToggleKinematics(armLPart.gameObject.transform, false);
        //enable colliders
        armLPart.GetComponent<PhysicsIKRig>().enabled = false;
        armLPart.GetComponent<Animator>().enabled = false;
        armL.transform.parent = null;
        armL = null;
        armLPart.creature = null;

        recalculate();
    }

    private void detachArmR()
    {
        armRPart.ToggleKinematics(armRPart.gameObject.transform, false);
        //enable colliders
        armRPart.GetComponent<PhysicsIKRig>().enabled = false;
        armRPart.GetComponent<Animator>().enabled = false;
        armR.transform.parent = null;
        armR = null;
        armRPart.creature = null;

        recalculate();
    }

    private void detachLegs()
    {
        ctrlsPart.GetComponent<RigidbodyController>().useFloat = false;
        //enable collider?
        legsPart.ToggleKinematics(legsPart.gameObject.transform, false);
        legsPart.GetComponent<LegIKRig>().enabled = false;
        legs.transform.parent = null;
        legs = null;
        legsPart.creature = null;

        recalculate();
    }

    #endregion

    #region Getters/Setters

    public float GetSpringStrengthLegs()
    {
        return springStrengthLegs;
    }

    public float GetSpringDamperLegs()
    {
        return springDamperLegs;
    }

    public void SetPlayerNum(int toSet)
    {
        playerNum = toSet;
    }

    public int GetPlayerNum()
    {
        return playerNum;
    }

    public float GetHealthHeadMax()
    {
        return healthHeadMax;
    }

    public float GetHealthTorsoMax()
    {
        return healthTorsoMax;
    }

    public float GetHealthArmLMax()
    {
        return healthArmLMax;
    }

    public float GetHealthArmRMax()
    {
        return healthArmRMax;
    }

    public float GetHealthLegsMax()
    {
        return healthLegsMax;
    }

    public float GetHealthHead()
    {
        return healthHead;
    }

    public float GetHealthTorso()
    {
        return healthTorso;
    }

    public float GetHealthArmL()
    {
        return healthArmL;
    }

    public float GetHealthArmR()
    {
        return healthArmR;
    }

    public float GetHealthLegs()
    {
        return healthLegs;
    }

    public float GetMass()
    {
        return mass;
    }

    public float GetJumpHeight()
    {
        return jumpHeight;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public float GetRotateSpeed()
    {
        return rotateSpeed;
    }

    public float GetStrengthArmL()
    {
        return strengthArmL;
    }

    public float GetStrengthArmR()
    {
        return strengthArmL;
    }

    public float GetSpringConstantArmL()
    {
        return springConstantArmL;
    }

    public float GetSpringConstantArmR()
    {
        return springConstantArmR;
    }

    public BodyPartData.animType GetAttackTypeL()
    {
        return attackTypeL;
    }

    public BodyPartData.animType GetAttackTypeR()
    {
        return attackTypeR;
    }

    public bool GetCanGrabL()
    {
        return canGrabL;
    }

    public bool GetCanGrabR()
    {
        return canGrabR;
    }

    #endregion

}
