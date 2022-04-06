using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is dynamically added to parts, and should never need to be manually attached.
public class JointCollision : MonoBehaviour
{
    public BodyPart bodyPart;
    public GameObject hitEffect;
    public AudioClip punch1;
    public AudioClip punch2;
    public AudioClip punch3;
    public AudioClip punch4;
    public AudioClip punch5;
    public AudioSource playSound;
    int punchAudio;

    void Start(){
        if (this.TryGetComponent<AudioSource>(out AudioSource audioNoise)){
            playSound = audioNoise;
        } else {
            playSound = gameObject.AddComponent<AudioSource>();
        }
        hitEffect = (GameObject)Resources.Load("punch effect", typeof(GameObject));
        punch1 = (AudioClip)Resources.Load("hit1.mp3", typeof(AudioClip));
        punch2 = (AudioClip)Resources.Load("hit2.mp3", typeof(AudioClip));
        punch3 = (AudioClip)Resources.Load("hit3.mp3", typeof(AudioClip));
        punch4 = (AudioClip)Resources.Load("hit4.mp3", typeof(AudioClip));
        punch5 = (AudioClip)Resources.Load("hit5.mp3", typeof(AudioClip));
    }

    private void OnCollisionEnter(Collision col)
    {
        //print("bodyPart: " + bodyPart);
        //print("bodyPart.creature: " + bodyPart.creature);
        //print("bodyPart.creature.alive: " + bodyPart.creature.alive);
        if (bodyPart.creature != null)
        {
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bodyPart.creature != null)
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
                    GameObject _hitEffect = Instantiate(hitEffect,other.transform.position, other.transform.rotation);
                    Destroy(_hitEffect, 1);
                    punchAudio = Random.Range( 1, 7);
                    if (punchAudio == 2){
                        playSound.clip = punch1;
                        playSound.Play();
                    }
                    else if (punchAudio == 3){
                        playSound.clip = punch2;
                        playSound.Play();
                    }
                    else if (punchAudio == 4){
                        playSound.clip = punch3;
                        playSound.Play();
                    }
                    else if (punchAudio == 5){
                        playSound.clip = punch4;
                        playSound.Play();
                    }
                    else if (punchAudio == 6){
                        playSound.clip = punch5;
                        playSound.Play();
                    }
                }
            }
        }
    }
}
