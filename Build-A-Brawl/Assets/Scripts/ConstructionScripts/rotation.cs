using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    IEnumerator RotateMe(Vector3 byAngles, float inTime) 
    {    var fromAngle = transform.rotation;
         var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
         for(var t = 0f; t < 1; t += Time.deltaTime/inTime) {
             transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
             yield return null;
        }
    }
    void Update () {

            StartCoroutine(RotateMe(Vector3.up * -90, 10f));
        
    }
}
