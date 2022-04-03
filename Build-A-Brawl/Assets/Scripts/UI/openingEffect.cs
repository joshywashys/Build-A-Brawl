using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openingEffect : MonoBehaviour
{

    public GameObject[] firstDelete;
    public GameObject[] secondDelete;
    public static bool shrinkStart = false;
    public float finalSize = 1;
    // Start is called before the first frame update
    void Start()
    {
        firstDelete = GameObject.FindGameObjectsWithTag("menuDelete");
        secondDelete = GameObject.FindGameObjectsWithTag("menuDeleteDelay");
    }

    // Update is called once per frame
    void Update()
    {
        if (shrinkStart ==true && finalSize > 0){
            foreach (GameObject laterDelete in secondDelete){
                laterDelete.transform.localScale = new Vector3(finalSize, finalSize, finalSize);
            }
            finalSize -= 0.001f;
        } else if (finalSize ==0 && shrinkStart ==true){
            foreach (GameObject laterDelete in secondDelete){
                Destroy(laterDelete);
                
            }
            shrinkStart = true;
        }
    }
    public void deleteEffect() {
        foreach (GameObject deleteMe in firstDelete){
            Destroy(deleteMe);
			knockBack();
        }
        shrinkStart = true;
        
    }

    void knockBack() {
		Collider[] colliders = Physics.OverlapSphere(transform.position, 20);

		foreach (Collider close in colliders){
			Rigidbody rigg = close.GetComponent<Rigidbody>();
			if (rigg != null){
				rigg.AddExplosionForce(4000, transform.position, 20);
			}
		}
	}
}
