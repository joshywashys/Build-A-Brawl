using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyBridge : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] bridge;
    

    void Start()
    {
        bridge = GameObject.FindGameObjectsWithTag("bridge");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "meteor"){
            bridge = GameObject.FindGameObjectsWithTag("bridge");
            foreach (GameObject panel in bridge)
                {
                    panel.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
    }
}
