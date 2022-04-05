using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable] public class DisableJoining : UnityEvent { }


public class disableJoining : MonoBehaviour
{

    public DisableJoining stopJoin;
    
    // Start is called before the first frame update
    void Start()
    {
        stopJoin.Invoke();
    }

}
