using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Track which UI element is selected, drag one for each part
public class HighlightLimb : MonoBehaviour
{
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

}
