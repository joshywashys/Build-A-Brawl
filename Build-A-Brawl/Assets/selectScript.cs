using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class selectScript : MonoBehaviour, ISelectHandler, IDeselectHandler, IEventSystemHandler
{ 
    public bool chkSelect;
    public bool notSelected;
    [SerializeField] public GameObject image;
    

    /*start with 2,
    
        on click, it will be 2 so chkselect changes to 

    */
    // Start is called before the first frame update
    void Start()
    {
        chkSelect = false;
        notSelected = true;
    }

    //if selected, with controller
    public void OnDeselect(BaseEventData eventData)
    {
        
            notSelected = true;
        
    }

    //if submiteed(button clicked on)
    public void OnClick(BaseEventData eventData)
    {
        //if it is not selected, change to pink
        if (!chkSelect)
        {
            //pink
            Debug.Log("I got cliked!");
        
            chkSelect = true;
            
            Debug.Log("Turn pink");
            return;
        } else if (chkSelect)
        {
            Debug.Log("IVE BEEN YELLOWED");
            //yellow
            
             chkSelect = false;
            Debug.Log("Turn yellow");
            return;
        }
      
    }

    public void Update()
    {
        //where upon the square is selected and the square is submitted
        if(!notSelected && !chkSelect)
        {
            //pink
            image.GetComponent<Image>().color = new Color32(255, 25, 216, 255);


            // where upon the square is not selected and the square is submitted 
        } else if (!notSelected && chkSelect)
        {
            
            //pink
            image.GetComponent<Image>().color = new Color32(255, 25, 216, 255);

          //where upon the square is not selected and the square is not submitted
        } else if (notSelected && !chkSelect)
        {
            
        //black
          image.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
            // where upon the square is selected and the square is not submitted 
        }
        else if (notSelected && chkSelect)
        {
            //yellow
            image.GetComponent<Image>().color = new Color32(255, 204, 52, 255);

        }
    }


    // if selected, with controller
    public void OnSelect(BaseEventData eventData)
    {
        image.GetComponent<Image>().color = new Color32(255, 25, 216, 255);

        Debug.Log("SELECTED!");
        notSelected = false;
    }
}
