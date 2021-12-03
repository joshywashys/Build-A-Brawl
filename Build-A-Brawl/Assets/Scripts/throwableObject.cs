using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwableObject : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static bool pickedUp = false;
    public Transform destination;

    void Start()
    {
        pickedUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp == true)
        {
            //collider on the object, when near can be picked up
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            this.transform.position = destination.position;
            this.transform.parent = GameObject.Find("Destination").transform;
        } else if (pickedUp == false)
        {
            this.transform.parent = GameObject.FindGameObjectWithTag("ThrowTo").transform;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}
