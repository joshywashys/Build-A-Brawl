using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class timerCount : MonoBehaviour
{
    public TextMeshProUGUI timerDisplay;
    public int timeRemain = 180;
    public bool countDown = false;

    void Start(){
        timerDisplay.text = timeRemain.ToString();
    }

    void Update(){
        if (countDown == false && timeRemain >0){
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer(){
        countDown = true;
        yield return new WaitForSeconds(1);
        timeRemain -= 1;
        timerDisplay.text = timeRemain.ToString();
        countDown = false;
    }
}
