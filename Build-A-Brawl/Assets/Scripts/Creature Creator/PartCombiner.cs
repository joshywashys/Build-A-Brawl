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
    private static GameObject[] headList;
    private GameObject[] torsoList;
    private GameObject[] armsList;
    private GameObject[] legsList;

    private GameObject currHead;
    private GameObject currTorso;
    private GameObject currArms;
    private GameObject currLegs;

    private float legsHeight;

    //we will recreate the creature every time a part is swapped out, despite it not being optimal, since it's not cpu-heavy at all anyways.
    //If it turns out to be cpu-heavy, we can optimize it to adjust the part locations.
    public void generateCreature()
    {
        //CURRENT BUG!!!!!!!!: leg GetHeight returns as 0 every time, but why?

        //calculations
        legsHeight = currLegs.GetComponent<Legs>().GetHeight();
        print(legsHeight);

        //instantiations
        Instantiate(currLegs, gameObject.transform);
        Instantiate(currTorso, new Vector3(0, legsHeight, 0), Quaternion.identity, gameObject.transform);
    }

    void Start()
    {
        //generate part lists:
        headList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Heads");
        torsoList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Torsos");
        armsList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Arms");
        legsList = Resources.LoadAll<GameObject>("Prefabs/CreatureParts/Legs");

        //set starting part to a random one
        currHead = headList[Random.Range(0,headList.Length)];
        currTorso = torsoList[Random.Range(0, torsoList.Length)];
        currArms = armsList[Random.Range(0, armsList.Length)];
        currLegs = legsList[Random.Range(0, legsList.Length)];
    }

    void Update()
    {
        
    }
}
