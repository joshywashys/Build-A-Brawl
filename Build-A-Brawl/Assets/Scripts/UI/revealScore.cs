using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class revealScore : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject firstPlace;
    public GameObject secondPlace;
    public GameObject thirdPlace;
    public GameObject fourthPlace;

    public AudioSource p1;
    public AudioSource p2;
    public AudioSource p3;
    public AudioSource p4;
    
    int introduceInt = 1;

    void Start()
    {
            InvokeRepeating("introduce", 16.0f, 0.8f);
    }

    void introduce(){
            
            if (introduceInt == 1){
                Debug.Log("test");
                firstPlace.SetActive(true);
                p1.Play();
                introduceInt++;
            } else if (introduceInt == 2){
                Debug.Log("test");
                secondPlace.SetActive(true);
                p2.Play();
                introduceInt++;
            }
            else if (introduceInt == 3){
                Debug.Log("test");
                thirdPlace.SetActive(true);
                p3.Play();
                introduceInt++;
            }
            else if (introduceInt == 4){
                Debug.Log("test");
                fourthPlace.SetActive(true);
                p4.Play();
                CancelInvoke("introduce");
            }
        }

    // Update is called once per frame
    void Update()
    {
        
    }
}
