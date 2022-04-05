using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punchGlass : MonoBehaviour
{
    public GameObject glassBits;
    public AudioSource shatter1;
    public AudioSource shatter2;
    public AudioSource shatter3;
    public AudioSource shatter4;
    int shatterSelect;
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
            shatterSelect = Random.Range( 1, 4);
            if (shatterSelect == 1){
                shatter1.Play();
            }
            else if (shatterSelect == 2){
                shatter2.Play();
            }
            else if (shatterSelect == 3){
                shatter3.Play();
            }
            else if (shatterSelect == 4){
                shatter4.Play();
            }
        
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collision) {
                if (collision.gameObject.tag == "heavyAttack" || collision.gameObject.tag == "ground"){
                GameObject _bits = Instantiate(glassBits,transform.position, transform.rotation);
                Destroy(_bits, 5);
                shatterSelect = Random.Range( 1, 4);
                if (shatterSelect == 1){
                    shatter1.Play();
                }
                else if (shatterSelect == 2){
                    shatter2.Play();
                }
                else if (shatterSelect == 3){
                    shatter3.Play();
                }
                else if (shatterSelect == 4){
                    shatter4.Play();
                }
            
                Destroy(gameObject);
            }
     }
}
