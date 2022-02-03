using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HOW TO USE/PURPOSE
//During runtime, this is the mothership data script. All of a creature's data will be imported here from it's parts so we don't have to access each individual part.
public class CreatureStats : MonoBehaviour
{
    // part references
    private GameObject creature;

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
    [SerializeField] private float mass = 10;
    [SerializeField] private float jumpHeight = 10; //total mass + leg strength
    [SerializeField] private float moveSpeed = 10; //total mass + leg speed
    [SerializeField] private float rotateSpeed = 10; //strength of all parts + mass

    // Arm Stats
    [Header("Arm Stats")]
    [SerializeField] private float strengthArmL = 10;
    [SerializeField] private float strengthArmR = 10;
    [SerializeField] private float strengthArms = 20;
    [SerializeField] private float springConstantArmL = 80;
    [SerializeField] private float springConstantArmR = 80;
    [SerializeField] private int attackTypeL = 0; //0 uses the default system, others would be robot sticks out arms, druid flails/spins around
    [SerializeField] private int attackTypeR = 0;
    [SerializeField] private bool canGrabL = true;
    [SerializeField] private bool canGrabR = true;

    #region  Internal Functions

    //call when a creature's stats need to be updated (when a limb is knocked off)
    private void recalculate()
    {
        // Mass
        float headMassNew, torsoMassNew, armLMassNew, armRMassNew, legsMassNew;
        headMassNew = torsoMassNew = armLMassNew = armRMassNew = legsMassNew = 0;

        if (head != null) { headMassNew = headPart.getMass(); }
        if (torso != null) { torsoMassNew = torsoPart.getMass(); }
        if (armL != null) { armLMassNew = armLPart.getMass(); }
        if (armR != null) { armRMassNew = armRPart.getMass(); }
        if (legs != null) { legsMassNew = legsPart.getMass(); }

        mass = headMassNew + torsoMassNew + armLMassNew + armRMassNew + legsMassNew;

        jumpHeight = legsPart.getStrength() / mass;
        moveSpeed = legsPart.getStrength() / mass;
        rotateSpeed = torsoPart.getMass() / mass;

    }

    //get parts stats and load them onto this script
    private void initializeCreature()
    {
        // Set refs
        creature = gameObject;
        headPart = head.GetComponent<BodyPart>();
        torsoPart = torso.GetComponent<BodyPart>();
        armLPart = armL.GetComponent<BodyPart>();
        armRPart = armR.GetComponent<BodyPart>();
        legsPart = legs.GetComponent<BodyPart>();

        // Health pools
        healthHead = healthHeadMax = headPart.getHealth();
        healthTorso = healthTorsoMax = torsoPart.getHealth();
        healthArmL = healthArmLMax = armLPart.getHealth();
        healthArmR = healthArmRMax = armRPart.getHealth();
        healthLegs = healthLegsMax = legsPart.getHealth();

        // Creature Stats
        mass = headPart.getMass() + torsoPart.getMass() + armLPart.getMass() + armRPart.getMass() + legsPart.getMass();
        jumpHeight = legsPart.getStrength() / mass;
        moveSpeed = legsPart.getStrength() / mass;
        rotateSpeed = legsPart.getStrength() / mass;

        // Arm Stats
        strengthArmL = armLPart.getStrength();
        strengthArmR = armRPart.getStrength();
        strengthArms = strengthArmL + strengthArmR;
        springConstantArmL = armLPart.getSpringConstant();
        springConstantArmR = armRPart.getSpringConstant();

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
        initializeCreature();
        //detachTorso(); //debugging
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

    #region Getters

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

    public int GetAttackTypeL()
    {
        return attackTypeL;
    }

    public int GetAttackTypeR()
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
