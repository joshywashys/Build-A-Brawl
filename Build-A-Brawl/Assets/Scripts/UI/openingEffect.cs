using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openingEffect : MonoBehaviour
{

    public GameObject[] firstDelete;
    public GameObject[] secondDelete;
    // Start is called before the first frame update
    void Start()
    {
        firstDelete = GameObject.FindGameObjectsWithTag("menuDelete");
        secondDelete = GameObject.FindGameObjectsWithTag("menuDeleteDelay");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void deleteEffect() {
        foreach (GameObject deleteMe in firstDelete){
            Destroy(deleteMe);
			knockBack();
        }
    }

    void knockBack() {
		Collider[] colliders = Physics.OverlapSphere(transform.position, 20);

		foreach (Collider close in colliders){
			Rigidbody rigg = close.GetComponent<Rigidbody>();
			if (rigg != null){
				rigg.AddExplosionForce(1500, transform.position, 20);
			}
		}
	}
}
