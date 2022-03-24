using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyBridge1 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] bridge1;
    

    void Start()
    {
        bridge1 = GameObject.FindGameObjectsWithTag("bridge1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "meteor"){
            bridge1 = GameObject.FindGameObjectsWithTag("bridge1");
            foreach (GameObject panel in bridge1)
                {
                    panel.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
    }
}
