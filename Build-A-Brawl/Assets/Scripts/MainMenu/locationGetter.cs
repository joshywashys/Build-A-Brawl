using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class locationGetter : MonoBehaviour
{

    GameObject newLocation;

    public void getLocation(GameObject trueLocation)
    {
        newLocation = trueLocation;
    }
}
