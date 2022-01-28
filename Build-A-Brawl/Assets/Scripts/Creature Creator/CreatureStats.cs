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
    float healthHead;
    float healthTorso;
    float healthShoulderL;
    float healthShoulderR;
    float healthLegs;

    // creature stats
    float strengthArmL;
    float strengthArmR;
    float jumpHeight; //calculated from total mass + leg strength
    float moveSpeed; //calculated from total mass + leg speed, recalculated whenever a part detaches
    float rotateSpeed;

    void Start()
    {
        creature = gameObject;
        initializeCreature();
        detachPart(torso);
    }

    void Update()
    {
        
    }

    //INTERNAL FUNCTIONS

    //get parts stats and load them onto this script
    private void initializeCreature()
    {

    }

    public void attachParts(GameObject newHead, GameObject newTorso, GameObject newArmL, GameObject newArmR, GameObject newLegs)
    {
        head = newHead;
        torso = newTorso;
        armL = newArmL;
        armR = newArmR;
        legs = newLegs;
    }

    private void detachPart(GameObject toDetach)
    {
        toDetach.transform.parent = null;
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
