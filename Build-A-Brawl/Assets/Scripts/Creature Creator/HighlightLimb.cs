using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Track which UI element is selected, drag one for each part
public class HighlightLimb : MonoBehaviour
{
    public GameObject arrows;
    public GameObject arrowL;
    public GameObject arrowR;
    public Vector3 currPos;

    enum Limbs
    {
        head,
        torso,
        armL,
        armR,
        legs
    }
    Limbs limb;

    private PartCombiner partCombiner;

    private void Start()
    {
        currPos = partCombiner.gameObject.transform.position;
    }

    public void RemoveHighlight()
    {
        arrows.SetActive(false);
    }

    //drag this onto all partcombiner OnPartSwap Events
    public void AddHighlight()
    {
        arrows.SetActive(true);
        if (limb == Limbs.head)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newHead.transform.position.y, currPos.z);
            arrows.transform.position = partCombiner.newHead.transform.position;
        }
        if (limb == Limbs.torso)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newTorso.transform.position.y, currPos.z);
            arrows.transform.position = partCombiner.newTorso.transform.position;
        }
        if (limb == Limbs.armL)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newArmL.transform.position.y, currPos.z);
            arrows.transform.position = partCombiner.newArmL.transform.position;
        }
        if (limb == Limbs.armR)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newArmR.transform.position.y, currPos.z);
            arrows.transform.position = partCombiner.newArmR.transform.position;
        }
        if (limb == Limbs.legs)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newLegs.transform.position.y, currPos.z);
            arrows.transform.position = partCombiner.newLegs.transform.position;
        }

        /*
        if (limb == Limbs.head)
        {
            partCombiner.newHead.AddComponent<PlayerOutline>();
        }
        if (limb == Limbs.torso)
        {
            partCombiner.newTorso.AddComponent<PlayerOutline>();
        }
        if (limb == Limbs.armL)
        {
            partCombiner.newArmL.AddComponent<PlayerOutline>();
        }
        if (limb == Limbs.armR)
        {
            partCombiner.newArmR.AddComponent<PlayerOutline>();
        }
        if (limb == Limbs.legs)
        {
            partCombiner.newLegs.AddComponent<PlayerOutline>();
        }
        */
    }

}
