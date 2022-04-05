using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateText : MonoBehaviour
{
    public Text text;

    // Add this to an event on another script for it to be called.
    public void ChangeText(string newText)
    {
        text.text = newText;
    }
        
    public void Start()
    {
        text = gameObject.GetComponent<Text>();
    }

}
