using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HOW TO USE/PURPOSE
//During runtime, this is the mothership script. All of a creature's data will be imported here from it's parts so we don't have to access each individual part.
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
    float healthShoulderL;
    float healthShoulderR;
    float healthLegs;

    // creature stats
    float mass;
    float strengthArmL;
    float strengthArmR; 
    float jumpHeight; //total mass + leg strength
    float moveSpeed; //total mass + leg speed
    float rotateSpeed; //strength of all parts + mass

    int attackTypeL; //0 uses the default system, others would be robot sticks out arms, druid flails/spins around
    int attackTypeR;
    bool canGrabL;
    bool canGrabR;

    void Start()
    {
        creature = gameObject;
        initializeCreature();
        //detachPart(torso);
    }

    void Update()
    {
        
    }

    //INTERNAL FUNCTIONS

    //get parts stats and load them onto this script
    private void initializeCreature()
    {

    }

    //call when a creature's stats need to be updated (when a limb is knocked off)
    private void calculateStats()
    {

    }

    //initialize
    public void attachParts(GameObject newHead, GameObject newTorso, GameObject newArmL, GameObject newArmR, GameObject newLegs)
    {
        head = newHead;
        torso = newTorso;
        armL = newArmL;
        armR = newArmR;
        legs = newLegs;
    }

    //recalculate
    private void detachPart(GameObject toDetach)
    {
        toDetach.transform.parent = null;
        //toDetach = null;
        calculateStats();
    }

    //TEMP
    private void debugDetachTorso()
    {
        torso.transform.parent = null;
    }

    //GETTERS
    public List<GameObject> getPartList()
    {
        return parts;
    }

}
