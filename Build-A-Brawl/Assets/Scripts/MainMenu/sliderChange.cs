using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class sliderChange : MonoBehaviour
{
    public Slider charSelection;
    public Input player;
    public UnityEvent leftIndex;
    public UnityEvent rightIndex;


    [SerializeField] private GameObject lArrow1;

    [SerializeField] private GameObject rArrow1;

    void Start()
    {
        charSelection.onValueChanged.AddListener(delegate { ValueChangeCheck(); });       
    }
    IEnumerator wait()
            {
            yield return new WaitForSeconds(0.2f);
                lArrow1.GetComponent<Image>().color = new Color32(255, 255, 225, 225);
                rArrow1.GetComponent<Image>().color = new Color32(255, 255, 225, 225);
                
            }
    public void ValueChangeCheck()
    {
        //when value changes, the slider is returned to the center right away so
        // the player must keep pushing the slider to continue selection
        // this prevents the player from having to do the switch on their own
        // back to neutral

        Debug.Log(charSelection.value);
        if (charSelection.value == 1)
        {
            leftIndex.Invoke();
            Debug.Log("left Index invoked");
            lArrow1.GetComponent<Image>().color = new Color32(168, 165, 87, 255);
            charSelection.value = 2;
            
        } else if (charSelection.value == 3) {
            rightIndex.Invoke();
            rArrow1.GetComponent<Image>().color = new Color32(168, 165, 87, 255);
            Debug.Log("right index invoked");
            charSelection.value = 2;
          
        } else
        {
            Debug.Log("Neutral");
            StartCoroutine(wait());
        }

    }
}
