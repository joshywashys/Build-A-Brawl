using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lowering : MonoBehaviour
{
    public float mass;

    public Rigidbody ball;
    public AudioSource concreteSmash1;
    public AudioSource concreteSmash2;
    public AudioSource concreteSmash3;
    int concreteSmash;
    
    void Start()
    {
        mass = 1;
        InvokeRepeating("massChange", 1.0f, 40f);
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "concretePad"){
            concreteSmash = Random.Range( 1, 5);
            if (concreteSmash == 2){
                concreteSmash1.Play();
            }
            else if (concreteSmash == 3){
                concreteSmash2.Play();
            }
            else if (concreteSmash == 4){
                concreteSmash3.Play();
            }
        }

        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            Rigidbody rig = GetComponent<Rigidbody>();

            Vector3 collisionPoint = Vector3.zero;
            foreach (ContactPoint contact in collision.contacts)
                collisionPoint += contact.point;
            collisionPoint /= collision.contactCount;

            controller.SetState(PlayerController.State.Stunned);

            rig.AddExplosionForce(200.0f, collisionPoint, 5.0f);;
        }
        
    }

    void massChange()
    {
        ball.mass = mass;
        mass = mass + 10;
    }
}
