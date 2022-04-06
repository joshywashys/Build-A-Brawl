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
    public Vector3 focusPos;
    public Vector3 offsetPos;

    public enum Limbs
    {
        head,
        torso,
        armL,
        armR,
        legs
    }
    public Limbs limb;

    public PartCombiner partCombiner;

    private void Start()
    {
        currPos = partCombiner.gameObject.transform.position;
        offsetPos = new Vector3(0, 0, 2f);
    }

    public void RemoveHighlight()
    {
        arrows.SetActive(false);
    }

    public void Update()
    {
        if (limb == Limbs.head)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newHead.transform.position.y, currPos.z);
            //print(partCombiner.newHead.transform.localPosition);
            focusPos = partCombiner.creaturePlayable.transform.position + partCombiner.newHead.transform.localPosition;
        }
        if (limb == Limbs.torso)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newTorso.transform.position.y, currPos.z);
            //print(partCombiner.newTorso.transform.localPosition);
            focusPos = partCombiner.creaturePlayable.transform.position + partCombiner.newTorso.transform.localPosition;
        }
        if (limb == Limbs.armL)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newArmL.transform.position.y, currPos.z);
            //print(partCombiner.newArmL.transform.localPosition);
            focusPos = partCombiner.creaturePlayable.transform.position + partCombiner.newArmL.transform.localPosition;
        }
        if (limb == Limbs.armR)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newArmR.transform.position.y, currPos.z);
            focusPos = partCombiner.creaturePlayable.transform.position + partCombiner.newArmR.transform.localPosition;
        }
        if (limb == Limbs.legs)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newLegs.transform.position.y, currPos.z);
            focusPos = partCombiner.creaturePlayable.transform.position + partCombiner.newLegs.transform.localPosition;
        }
        arrows.transform.position = focusPos;
    }

    //drag this onto all partcombiner OnPartSwap Events
    public void AddHighlight()
    {
        //arrows.SetActive(true);

        /*
        if (limb == Limbs.head)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newHead.transform.position.y, currPos.z);
            focusPos = partCombiner.newHead.transform.position;
        }
        if (limb == Limbs.torso)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newTorso.transform.position.y, currPos.z);
            focusPos = partCombiner.newTorso.transform.position;
        }
        if (limb == Limbs.armL)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newArmL.transform.position.y, currPos.z);
            focusPos = partCombiner.newArmL.transform.position;
        }
        if (limb == Limbs.armR)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newArmR.transform.position.y, currPos.z);
            focusPos = partCombiner.newArmR.transform.position;
        }
        if (limb == Limbs.legs)
        {
            //arrows.transform.position = new Vector3(currPos.x, partCombiner.newLegs.transform.position.y, currPos.z);
            focusPos = partCombiner.newLegs.transform.position;
        }
        */

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
