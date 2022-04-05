using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StopJoin : UnityEvent {};
public class disableJoining : MonoBehaviour
{
    public UnityEvent StopJoin;

    // Start is called before the first frame update
    void Start()
    {
        StopJoin.Invoke();
    }

}
