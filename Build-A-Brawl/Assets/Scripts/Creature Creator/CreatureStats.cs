using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HOW TO USE/PURPOSE
//During runtime, this is the mothership data script. All of a creature's data will be imported here from it's parts so we don't have to access each individual part.
public class CreatureStats : MonoBehaviour
{
    // part references
    private GameObject creature;
    [SerializeField] private int playerNum;

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

        // Health pools
        healthHead = healthHeadMax = headPart.getHealthMultiplier();
        healthTorso = healthTorsoMax = torsoPart.getHealthMultiplier();
        healthArmL = healthArmLMax = armLPart.getHealthMultiplier();
        healthArmR = healthArmRMax = armRPart.getHealthMultiplier();
        healthLegs = healthLegsMax = legsPart.getHealthMultiplier();

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
    public void attachParts(GameObject newHead, GameObject newTorso, GameObject newArmL, GameObject newArmR, GameObject newLegs)
    {
        head = newHead;
        torso = newTorso;
        armL = newArmL;
        armR = newArmR;
        legs = newLegs;
    }

    #endregion

    #region MonoBehaviour Functions

    void Start()
    {
        //initializeCreature();
    }

    void Awake()
    {

    }

    #endregion

    #region Part Detaching

    private void detachHead()
    {
        detachTorso();

        head.transform.parent = null;
        head = null;
        recalculate();
    }

    private void detachTorso()
    {
        detachArmL();
        detachArmR();
        detachLegs();
        detachHead();

        torso.transform.parent = null;
        torso = null;
        recalculate();
    }

    private void detachArmL()
    {
        armL.transform.parent = null;
        armL = null;
        recalculate();
    }

    private void detachArmR()
    {
        armR.transform.parent = null;
        armR = null;
        recalculate();
    }

    private void detachLegs()
    {
        legs.transform.parent = null;
        legs = null;
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

    public GameObject GetArmL()
    {
        return armL;
    }

    public GameObject GetArmR()
    {
        return armR;
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
