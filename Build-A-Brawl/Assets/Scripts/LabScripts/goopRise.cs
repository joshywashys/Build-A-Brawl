using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goopRise : MonoBehaviour
{
    public GameObject goop;
    Vector3 rise = new Vector3(0,15,0);
     public Transform target;
     public float speed = 0.8f;
     bool goopRising = false;
     public int punchGoop = 0;
    // Start is called before the first frame update
    void Start()
    {
        goop = GameObject.FindGameObjectWithTag("goop");
    }

    // Update is called once per frame
    void Update()
    {
        if (goopRising) {
            //Debug.Log("goig up");
            float step = speed * Time.deltaTime;
            goop.transform.position = Vector3.MoveTowards(goop.transform.position, target.position, step);
            //Debug.Log("i am up");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "heavyAttack"){
            punchGoop++;
        }
        else if (collision.gameObject.tag == "heavyAttack" && punchGoop >= 7)
        {
            goopRising = true;
            //goop.transform.position = Vector3.MoveTowards(goop.transform.position, target, Time.deltaTime);
        }
    }
}
