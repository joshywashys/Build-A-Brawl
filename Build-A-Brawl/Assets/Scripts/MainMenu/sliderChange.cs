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

    [SerializeField] private GameObject generateLocate1;

    [SerializeField] private GameObject generateLocate2;

    [SerializeField] private GameObject generateLocate3;

    [SerializeField] private GameObject generateLocate4;

    void Start()
    {
        charSelection.onValueChanged.AddListener(delegate { ValueChangeCheck(); });       
    }

   public void accessLocation()
    {
    //    if ()
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
            charSelection.value = 2;
            
        } else if (charSelection.value == 3) {
            rightIndex.Invoke();
            Debug.Log("right index invoked");
            charSelection.value = 2;
          
        } else
        {
            Debug.Log("Neutral");
        }

    }
}
