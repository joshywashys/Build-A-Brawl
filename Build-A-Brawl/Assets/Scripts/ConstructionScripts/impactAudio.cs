using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class impactAudio : MonoBehaviour
{
    public AudioSource impact;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "concretePad"){
            impact.Play();
        }
    }
}
