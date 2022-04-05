using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blowUpCar : MonoBehaviour
{
    public GameObject explode, bits, smoke, flame;
    public float explosionStrength, limit;
    int health = 0;
    public AudioSource carBoom;
    // Start is called before the first frame update
    

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "heavyAttack"){
            health++;
            if (health==5){
                GameObject _smoke = Instantiate(smoke,transform.position, transform.rotation);
                Destroy(_smoke, 10);
            }else if (health==10){
                GameObject _flame = Instantiate(flame,transform.position, transform.rotation);
                Destroy(_flame, 10);
            }else if (health==15){
                GameObject _explosion = Instantiate(explode,transform.position, transform.rotation);
                GameObject _bits = Instantiate(bits,transform.position, transform.rotation);
                Destroy(_explosion, 3);
                Destroy(_bits, 5);
                knockBack();
                carBoom.Play();
                Destroy(gameObject);
            }
            
        }
    }

    private void OnTriggerEnter(Collider collision){
        if (collision.gameObject.tag == "heavyAttack"){
            health++;
            if (health==5){
                GameObject _smoke = Instantiate(smoke,transform.position, transform.rotation);
                Destroy(_smoke, 10);
            }else if (health==10){
                GameObject _flame = Instantiate(flame,transform.position, transform.rotation);
                Destroy(_flame, 10);
            }else if (health==15){
                GameObject _explosion = Instantiate(explode,transform.position, transform.rotation);
                GameObject _bits = Instantiate(bits,transform.position, transform.rotation);
                Destroy(_explosion, 3);
                Destroy(_bits, 5);
                knockBack();
                carBoom.Play();
                Destroy(gameObject);
            }
            
        }
    }

    void knockBack() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, limit);

        foreach (Collider close in colliders){
            if (close.TryGetComponent<PlayerController>(out PlayerController controller))
				controller.SetState(PlayerController.State.Stunned);
                
            Rigidbody rigg = close.GetComponent<Rigidbody>();
            if (rigg != null){
                rigg.AddExplosionForce(explosionStrength, transform.position, limit);
            }
        }
    }
}
