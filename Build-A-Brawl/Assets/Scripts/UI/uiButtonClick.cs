using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiButtonClick : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource click;
    public AudioSource back;
    public AudioSource little;
    public AudioSource change;
    public AudioSource breakthrough;
    public static bool firstClick = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickButton(){
        click.Play();
        if (firstClick == true){
            breakthrough.Play();
            firstClick = false;
        }
        
    }
    public void backButton(){
        back.Play();
    }
    public void smallButton(){
        little.Play();
    }
    public void changeButton(){
        change.Play();
    }
}
