using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class overlayInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    //public Image limb;
    public Image limbOverlay;
    public Color hitColor1;
    public Color hitColor2;
    public Color hitColor3;
    public Color hitColor4;
    public Color hitColor5;
    public Color hitColor6;
    public int hit = 0;
    public int totalHealth = 0;
    void Start()
    {
        hitColor1.a = 1;
        hitColor2.a = 1;
        hitColor3.a = 1;
        hitColor4.a = 1;
        hitColor5.a = 1;
        hitColor6.a = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit > 3){
            totalHealth++;
            hit = 0;
            Debug.Log(totalHealth);
            if (totalHealth == 1){
                limbOverlay.color = hitColor1;
            } else if (totalHealth == 1){
                limbOverlay.color = hitColor2;
            } else if (totalHealth == 2){
                limbOverlay.color = hitColor3;
            } else if (totalHealth == 3){
                limbOverlay.color = hitColor4;
            } else if (totalHealth == 4){
                limbOverlay.color = hitColor5;
            } else if (totalHealth == 5){
                limbOverlay.color = hitColor6;
            }
        }
        if (totalHealth == 6){
            Debug.Log("DETACH LIMB");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision detected");
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "heavyAttack")
        {
            hit++;
            Debug.Log(hit);
        }
    }
}
