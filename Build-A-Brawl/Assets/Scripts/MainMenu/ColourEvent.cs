using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColourEvent : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] public GameObject image;
    private bool colourState = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //on button selection
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(this.gameObject.name + " selected");
        colourState = true;

       
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log(this.gameObject.name + " deselected");
        colourState = false;
        
    }

    // Update is called once per frame
    void Update()
    {
       

        if (colourState)
        {
            image.GetComponent<Image>().color = new Color32(255, 0, 204, 255);
        } else if (!colourState) {

          image.GetComponent<Image>().color = new Color32 (255, 204, 52, 255);
 
        }
    }
}
