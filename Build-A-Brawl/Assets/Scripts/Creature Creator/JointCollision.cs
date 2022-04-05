using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is dynamically added to parts, and should never need to be manually attached.
public class JointCollision : MonoBehaviour
{
    public BodyPart bodyPart;

    private void OnCollisionEnter(Collision col)
    {
        //print("bodyPart: " + bodyPart);
        //print("bodyPart.creature: " + bodyPart.creature);
        //print("bodyPart.creature.alive: " + bodyPart.creature.alive);
        if (bodyPart.creature.alive)
        {
            float force = col.impulse.magnitude;

            //if (col.gameObject == bodyPart.creature.ctrlsPart.GetComponent<PlayerController>().fistLeftRigidbody.gameObject) { print("left hit COL"); }
            //if (col.gameObject == bodyPart.creature.ctrlsPart.GetComponent<PlayerController>().fistRightRigidbody.gameObject) { print("right hit COL"); }
            if (col.gameObject.tag == "heavyAttack" && col.gameObject != bodyPart.creature.ctrlsPart.GetComponent<PlayerController>().fistLeftRigidbody.gameObject && col.gameObject != bodyPart.creature.ctrlsPart.GetComponent<PlayerController>().fistRightRigidbody.gameObject)
            {
                //print(col.gameObject);
                if (bodyPart.creature != null) { bodyPart.creature.Damage(bodyPart.partData, force); }
            }
            if (col.gameObject.tag == "hazard" || col.gameObject.tag == "goop")
            {
                //print("heavyAttack collision: " + gameObject.name + " --- impulse: " + col.impulse);
                //print(bodyPart.partData);
                if (bodyPart.creature != null) { bodyPart.creature.Kill(); }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bodyPart.creature.alive)
        {
            if (other.gameObject.tag == "hazard" || other.gameObject.tag == "goop")
            {
                //print("heavyAttack collision: " + gameObject.name + " --- impulse: " + col.impulse);
                //print(bodyPart.partData);
                if (bodyPart.creature != null) { bodyPart.creature.Kill(); }
            }

            //float force = other.impulse.magnitude;

            //if (other.gameObject == bodyPart.creature.ctrlsPart.GetComponent<PlayerController>().fistLeftRigidbody.gameObject) { print("left hit"); }
            //if (other.gameObject == bodyPart.creature.ctrlsPart.GetComponent<PlayerController>().fistRightRigidbody.gameObject) { print("right hit"); }

            if (other.gameObject.tag == "heavyAttack" && other.gameObject != bodyPart.creature.ctrlsPart.GetComponent<PlayerController>().fistLeftRigidbody.gameObject && other.gameObject != bodyPart.creature.ctrlsPart.GetComponent<PlayerController>().fistRightRigidbody.gameObject)
            {
                print(other.gameObject);
                if (bodyPart.creature != null) { bodyPart.creature.Damage(bodyPart.partData, 1f); }
            }
        }
    }
}
