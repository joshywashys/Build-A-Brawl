using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable] public class Disabling : UnityEvent { }

public class disableJoining : MonoBehaviour
{
    public Disabling joinStart;

    private void Start()
    {
        joinStart.Invoke();
    }
}
