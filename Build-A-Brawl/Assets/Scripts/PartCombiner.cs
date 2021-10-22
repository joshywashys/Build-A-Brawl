using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
HOW/WHEN TO USE:
This script handles swapping around parts in the creature creator.
A separate script will be made to export the creature to a playable character.
 */
public class PartCombiner : MonoBehaviour
{
    public Transform generationLocation;

    private static GameObject[] headList;
    private GameObject[] torsoList;
    private GameObject[] armsList;
    private GameObject[] legsList;

    private GameObject currHead;
    private GameObject currTorso;
    private GameObject currArms;
    private GameObject currLegs;

    private GameObject newHead;
    private GameObject newTorso;
    private GameObject newArms;
    private GameObject newLegs;

    private float legsHeight;

    //we will recreate the creature every time a part is swapped out, despite it not being optimal, since it's not cpu-heavy at all anyways.
    //If it turns out to be cpu-heavy, we can optimize it to adjust the part locations.
    public void generateCreature()
    {
        //should probably do a bunch of checks to make sure each object has it's joints set up properly

        //spawn parts
        newTorso = Instantiate(currTorso, generationLocation.position, Quaternion.identity, generationLocation);
        newHead = Instantiate(currHead, generationLocation.position, Quaternion.identity, generationLocation);
        newLegs = Instantiate(currLegs, generationLocation.position, Quaternion.identity, generationLocation);

        //calculate where to move parts to attach to body parts
        float headToNeck = newHead.transform.position.y - newHead.transform.GetChild(0).transform.position.y;
        float torsoToNeck = newTorso.transform.position.y - newTorso.transform.GetChild(0).transform.position.y;

        float legsToHips = newLegs.transform.GetChild(0).transform.position.y - newLegs.transform.position.y;
        float torsoToHips = newTorso.transform.position.y - newTorso.transform.GetChild(3).transform.position.y;

        //move each part
        newHead.transform.Translate(0, headToNeck - torsoToNeck, 0);
        newLegs.transform.Translate(0, -(legsToHips + torsoToHips), 0);

        //shift creature up so that the bottom of the feet are at the spawnLocation
        //generationLocation.transform.Translate...

    }

    void Start()
    {
        //generate part lists:
        headList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Heads");
        torsoList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Torsos");
        armsList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Arms");
        legsList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Legs");

        //set starting part to be a random one
        currHead = headList[Random.Range(0,headList.Length)];
        currTorso = torsoList[Random.Range(0, torsoList.Length)];
        currArms = armsList[Random.Range(0, armsList.Length)];
        currLegs = legsList[Random.Range(0, legsList.Length)];
    }

    void Update()
    {
        
    }
}
