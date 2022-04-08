using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightLimbSender : MonoBehaviour
{
    public HighlightLimb.Limbs currLimb;
    public HighlightLimb script;

    public void SendLimb()
    {
        script.limb = currLimb;
    }
}
