using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jackHammer : MonoBehaviour
{
    public GameObject concreteBits;
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
        if (collision.gameObject.tag == "jackHammer" || collision.gameObject.tag == "bigBall")
        {
            GameObject _bits = Instantiate(concreteBits,transform.position, transform.rotation);
            Destroy(_bits, 5);
            Destroy(gameObject);
        }
    }
}
