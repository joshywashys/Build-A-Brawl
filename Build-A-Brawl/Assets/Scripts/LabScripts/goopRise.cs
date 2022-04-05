using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goopRise : MonoBehaviour
{
    public GameObject goop;
    Vector3 rise = new Vector3(0,15,0);
     public Transform target;
     public float speed = 0.8f;
     bool goopRising = false;
     public int punchGoop = 0;
    public GameObject explode, bits;
	public float explosionStrength, limit;
	public AudioSource boom;
    // Start is called before the first frame update
    void Start()
    {
        goop = GameObject.FindGameObjectWithTag("goop");
    }

    // Update is called once per frame
    void Update()
    {
        if (goopRising) {
            //Debug.Log("goig up");
            float step = speed * Time.deltaTime;
            goop.transform.position = Vector3.MoveTowards(goop.transform.position, target.position, step);
            //Debug.Log("i am up");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        
        if (collision.gameObject.tag == "heavyAttack" && punchGoop >= 7)
        {
            Debug.Log("GOOPY DOOPY");
            goopRising = true;
            GameObject _explosion = Instantiate(explode,transform.position, transform.rotation);
			GameObject _bits = Instantiate(bits,transform.position, transform.rotation);
			Destroy(_explosion, 3);
			Destroy(_bits, 5);
			knockBack();
            boom.Play();
            //goop.transform.position = Vector3.MoveTowards(goop.transform.position, target, Time.deltaTime);
        }
        else if (collision.gameObject.tag == "heavyAttack"){
            punchGoop++;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        
        if (collision.gameObject.tag == "heavyAttack" && punchGoop >= 7)
        {
            Debug.Log("GOOPY DOOPY");
            goopRising = true;
            GameObject _explosion = Instantiate(explode,transform.position, transform.rotation);
			GameObject _bits = Instantiate(bits,transform.position, transform.rotation);
			Destroy(_explosion, 3);
			Destroy(_bits, 5);
			knockBack();
            boom.Play();
            //goop.transform.position = Vector3.MoveTowards(goop.transform.position, target, Time.deltaTime);
        }
        else if (collision.gameObject.tag == "heavyAttack"){
            punchGoop++;
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
