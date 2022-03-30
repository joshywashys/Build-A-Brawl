using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punchGlass : MonoBehaviour
{
    public GameObject glassBits;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "heavyAttack" || collision.gameObject.tag == "ground")
        {
            GameObject _bits = Instantiate(glassBits,transform.position, transform.rotation);
            Destroy(_bits, 5);
            Destroy(gameObject);
        }
    }
}
