using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HOW TO USE/PURPOSE
//During runtime, this is the mothership data script. All of a creature's data will be imported here from it's parts so we don't have to access each individual part.
public class CreatureStats : MonoBehaviour
{
    // part references
    private GameObject creature;
    private List<GameObject> parts;
    private GameObject head;
    private GameObject torso;
    private GameObject armL;
    private GameObject armR;
    private GameObject legs;

    // <0 health means a part will be detached
    float healthHead; //creature dies at 0
    float healthTorso; //creature dies at 0?
    float healthArmL;
    float healthArmR;
    float healthLegs;

    // Creature Stats
    float mass = 10;
    float strengthArmL = 10;
    float strengthArmR = 10;
    float strengthArms = 20;
    float springConstantArmL = 80;
    float springConstantArmR = 80;
    float jumpHeight = 10; //total mass + leg strength
    float moveSpeed = 10; //total mass + leg speed
    float rotateSpeed = 10; //strength of all parts + mass

    int attackTypeL; //0 uses the default system, others would be robot sticks out arms, druid flails/spins around
    int attackTypeR;
    bool canGrabL = true;
    bool canGrabR = true;

    #region MonoBehaviour Functions

    void Start()
    {
        creature = gameObject;
        initializeCreature();
        //detachTorso(); //debugging
    }

    #endregion

    #region  Internal Functions

    //get parts stats and load them onto this script
    private void initializeCreature()
    {
        recalculate();
    }

    //call when a creature's stats need to be updated (when a limb is knocked off)
    private void recalculate()
    {
        //get part refs
        BodyPart headPart = head.GetComponent<BodyPart>();
        BodyPart torsoPart = head.GetComponent<BodyPart>();
        BodyPart armLPart = head.GetComponent<BodyPart>();
        BodyPart armRPart = head.GetComponent<BodyPart>();
        BodyPart legsPart = head.GetComponent<BodyPart>();

        healthHead = headPart.getHealth();
        healthTorso = torsoPart.getHealth();
        healthArmL = armLPart.getHealth();
        healthArmR = armRPart.getHealth();
        healthLegs = legsPart.getHealth();

        mass = headPart.getMass() + torsoPart.getMass() + armLPart.getMass() + armRPart.getMass() + legsPart.getMass();
        strengthArmL = armLPart.getStrength();
        strengthArmR = armRPart.getStrength();
        strengthArms = strengthArmL + strengthArmR;
        springConstantArmL = armLPart.getSpringConstant();
        springConstantArmR = armRPart.getSpringConstant();

        jumpHeight = legsPart.getStrength() / mass;
        moveSpeed = legsPart.getStrength() / mass;

        //still have to do canGrab and attackTypes
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
    public List<GameObject> getPartList()
    {
        return parts;
    }
    #endregion

}
