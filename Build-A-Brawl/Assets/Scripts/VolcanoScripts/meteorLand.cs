using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteorLand : MonoBehaviour
{
    public GameObject explode, crater, craterSpawn;
    public float explosionStrength, limit;
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
