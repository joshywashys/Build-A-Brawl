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
    public GameObject riseGas;
    public GameObject hurtGas;
    public GameObject target;
    public bool hit = false;

    void Start(){
        timerDisplay.text = timeRemain.ToString();
        InvokeRepeating("bigHurt", 180, 2.5f);
    }

    void Update(){
        if (countDown == false && timeRemain >0){
            StartCoroutine(Timer());
        }
        if (timeRemain == 0){
            timerDisplay.text = "Poison!";
            float step = 5.0f * Time.deltaTime;
            riseGas.transform.position = Vector3.MoveTowards(riseGas.transform.position, target.transform.position, step);
        }
    }

    void bigHurt(){
        if (hit == true){
            hurtGas.SetActive(true);
            hit = false;
        }
        else if (hit == false){
            hurtGas.SetActive(false);
            hit = true;
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
