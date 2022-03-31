using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beginCountdown : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject num3, num2, num1, brawl;
    int count = 3;
    
    public AudioSource startSound;
    void Start()
    {
        InvokeRepeating("countDown", 0.0f, 0.9f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void countDown(){
        if (count == 3){
            GameObject number3 = Instantiate(num3,transform.position, transform.rotation);
            Destroy(number3, 0.5f);
            startSound.Play();
            count =2;
        } else if (count == 2){
            GameObject number2 = Instantiate(num2,transform.position, transform.rotation);
            Destroy(number2, 0.5f);
            count =1;
        } else if (count == 1){
            GameObject number1 = Instantiate(num1,transform.position, transform.rotation);
            Destroy(number1, 0.5f);
            count =0;
        }
        else if (count == 0){
            GameObject brawlTxt = Instantiate(brawl,transform.position, transform.rotation);
            Destroy(brawlTxt, 0.5f);
            CancelInvoke("countDown");
        }

    }
}
