using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using TMPro;

public class IncDecNum : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI timerNum;
    public TextMeshProUGUI roundNum;
    public AudioSource noThanks;
    public int timerVal;
    public int roundVal;
    public int timerActual;
    void Start()
    {
        timerVal = 3;
        timerActual = 180;
        roundVal = 6;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void timerInc(){
        if (timerVal < 10){
            timerVal ++;
            timerActual += 60;
            timerNum.text = timerVal.ToString();
        } else {
            noThanks.Play();
        }
    }
    public void timerDec(){
        if (timerVal > 1){
            timerVal --;
            timerActual -= 60;
            timerNum.text = timerVal.ToString();
        } else {
            noThanks.Play();
        }
    }
    public void roundInc(){
        if (roundVal < 10){
            roundVal ++;
            roundNum.text = roundVal.ToString();
        } else {
            noThanks.Play();
        }
    }
    public void roundDec(){
        if (roundVal > 1){
            roundVal --;
            roundNum.text = roundVal.ToString();
        } else {
            noThanks.Play();
        }
    }
}
