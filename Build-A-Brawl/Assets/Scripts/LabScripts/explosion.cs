using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    public GameObject explode, bits;
    public float explosionStrength, limit;


    [Header("Mechanical Variables")]
    public bool useMechanics = false;
    public float dmg; //set to 0 for a no damage explosion
    public float kb; //set to 0 for a no knockback explosion

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "heavyAttack" || collision.gameObject.tag == "ground"){

            GameObject _explosion = Instantiate(explode,transform.position, transform.rotation);
            GameObject _bits = Instantiate(bits,transform.position, transform.rotation);
            Destroy(_explosion, 3);
            Destroy(_bits, 5);
            knockBack();
            Destroy(gameObject);
        }

        if (useMechanics)
        {
            //inverse square law, apply collision to players in radius. force = knockback
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

    //for knockback
    void spawnExplosionTriggerSphere()
    {

    }
}
