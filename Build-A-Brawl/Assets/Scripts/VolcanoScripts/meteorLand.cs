using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteorLand : MonoBehaviour
{
    public GameObject explode, crater, craterSpawn;
    public float explosionStrength, limit;
    public AudioSource land1;
    public AudioSource land2;
    int landSound;
    // Start is called before the first frame update
    

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "ground"){

            GameObject _explosion = Instantiate(explode,transform.position, transform.rotation);
            GameObject _crater = Instantiate(crater,craterSpawn.transform.position, transform.rotation);
            Destroy(_explosion, 3);
            knockBack();
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "bridge" || collision.gameObject.tag == "bridge1"){

            GameObject _explosion = Instantiate(explode,transform.position, transform.rotation);
            Destroy(_explosion, 3);
            knockBack();
            Destroy(gameObject);
        }
        landSound = Random.Range( 1, 3);
        Debug.Log(landSound);
        if (landSound == 1){
            land1.Play();
        }
        else if (landSound == 2){
            land2.Play();
        }
    }

    void knockBack() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, limit);

        foreach (Collider close in colliders){
            Rigidbody rigg = close.GetComponent<Rigidbody>();
            if (rigg != null){
                rigg.AddExplosionForce(explosionStrength, transform.position, limit);
            }
        }
    }
}
