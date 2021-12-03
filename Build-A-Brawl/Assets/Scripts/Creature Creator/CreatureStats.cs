using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HOW TO USE/PURPOSE
//During gameplay, this is the mothership script. All of a creature's data will be imported here from it's parts (so we don't have to access each individual part).
public class CreatureStats : MonoBehaviour
{
    private GameObject creature;
    private GameObject head;
    private GameObject torso;
    private GameObject armL;
    private GameObject armR;
    private GameObject legs;

    // Start is called before the first frame update
    void Start()
    {
        creature = gameObject;
        attachParts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void getParts()
    {

    }

    private void attachParts()
    {
        head = creature.transform.GetComponentInChildren<Head>().gameObject;
        torso = creature.transform.GetComponentInChildren<Torso>().gameObject;
        armL = creature.transform.GetComponentInChildren<ArmL>().gameObject;
        armR = creature.transform.GetComponentInChildren<ArmR>().gameObject;
        legs = creature.transform.GetComponentInChildren<Legs>().gameObject;
    }
}
