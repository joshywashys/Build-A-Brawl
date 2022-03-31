using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrelNoise : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource bonk1;
    public AudioSource bonk2;
    public AudioSource bonk3;
    public AudioSource bonk4;
    int bonkSound;

    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "ground"){
            bonkSound = Random.Range( 1, 5);
            if (bonkSound == 1){
                bonk1.Play();
            }
            else if (bonkSound == 2){
                bonk2.Play();
            }
            else if (bonkSound == 3){
                bonk3.Play();
            }
            else if (bonkSound == 4){
                bonk4.Play();
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
