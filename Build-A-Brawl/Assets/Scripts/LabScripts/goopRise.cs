using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goopRise : MonoBehaviour
{
    public GameObject goop;
    Vector3 rise = new Vector3(0,15,0);
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        goop = GameObject.FindGameObjectWithTag("goop");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "heavyAttack")
        {
            goop.transform.Translate(Vector3.up * Time.deltaTime);
            //goop.transform.position = Vector3.MoveTowards(goop.transform.position, target, Time.deltaTime);
        }
    }
}
