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
    }

    public void RemoveHighlight()
    {
        if (limb == Limbs.head)
        {
            Destroy(partCombiner.newHead.GetComponent<PlayerOutline>());
        }
        if (limb == Limbs.torso)
        {
            Destroy(partCombiner.newTorso.GetComponent<PlayerOutline>());
        }
        if (limb == Limbs.armL)
        {
            Destroy(partCombiner.newArmL.GetComponent<PlayerOutline>());
        }
        if (limb == Limbs.armR)
        {
            Destroy(partCombiner.newArmR.GetComponent<PlayerOutline>());
        }
        if (limb == Limbs.legs)
        {
            Destroy(partCombiner.newLegs.GetComponent<PlayerOutline>());
        }
    }

}
