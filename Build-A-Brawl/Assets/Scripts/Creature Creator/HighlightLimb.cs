using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Track which UI element is selected, drag one for each part
public class HighlightLimb : MonoBehaviour
{
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

    //drag this onto all partcombiner OnPartSwap Events
    public void AddHighlight()
    {
        
        if (limb == Limbs.head)
        {
            if (partCombiner.newHead.GetComponent<PlayerOutline>() != null) { partCombiner.newHead.GetComponent<PlayerOutline>().enabled = true; }
        }
        if (limb == Limbs.torso)
        {
            if (partCombiner.newTorso.GetComponent<PlayerOutline>() != null) { partCombiner.newTorso.GetComponent<PlayerOutline>().enabled = true; }
        }
        if (limb == Limbs.armL)
        {
            if (partCombiner.newArmL.GetComponent<PlayerOutline>() != null) { partCombiner.newArmL.GetComponent<PlayerOutline>().enabled = true; }
        }
        if (limb == Limbs.armR)
        {
            if (partCombiner.newArmR.GetComponent<PlayerOutline>() != null) { partCombiner.newArmR.GetComponent<PlayerOutline>().enabled = true; }
        }
        if (limb == Limbs.legs)
        {
            if (partCombiner.newLegs.GetComponent<PlayerOutline>() != null) { partCombiner.newLegs.GetComponent<PlayerOutline>().enabled = true; }
        }
        
    }

    public void RemoveHighlight()
    {
        if (limb == Limbs.head)
        {
            if (partCombiner.newHead.GetComponent<PlayerOutline>() != null) { partCombiner.newHead.GetComponent<PlayerOutline>().enabled = false; }
        }
        if (limb == Limbs.torso)
        {
            if (partCombiner.newTorso.GetComponent<PlayerOutline>() != null) { partCombiner.newTorso.GetComponent<PlayerOutline>().enabled = false; }
        }
        if (limb == Limbs.armL)
        {
            if (partCombiner.newArmL.GetComponent<PlayerOutline>() != null) { partCombiner.newArmL.GetComponent<PlayerOutline>().enabled = false; }
        }
        if (limb == Limbs.armR)
        {
            if (partCombiner.newArmR.GetComponent<PlayerOutline>() != null) { partCombiner.newArmR.GetComponent<PlayerOutline>().enabled = false; }
        }
        if (limb == Limbs.legs)
        {
            if (partCombiner.newLegs.GetComponent<PlayerOutline>() != null) { partCombiner.newLegs.GetComponent<PlayerOutline>().enabled = false; }
        }

    }

}
