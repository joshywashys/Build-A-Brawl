using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is dynamically added to parts, and should never need to be manually attached.
public class JointCollision : MonoBehaviour
{
    public BodyPart bodyPart;

    private void OnCollisionEnter(Collision col)
    {
        float force = col.impulse.magnitude;

        if (col.gameObject.tag == "heavyAttack")
        {
            //print("heavyAttack collision: " + gameObject.name + " --- impulse: " + col.impulse);
            //print(bodyPart.partData);
            if (bodyPart.creature != null) { bodyPart.creature.Damage(bodyPart.partData, force); }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "hazard" || other.gameObject.tag == "goop")
        {
            //print("heavyAttack collision: " + gameObject.name + " --- impulse: " + col.impulse);
            //print(bodyPart.partData);
            if (bodyPart.creature != null) { bodyPart.creature.Kill(); }
        }
    }
}
