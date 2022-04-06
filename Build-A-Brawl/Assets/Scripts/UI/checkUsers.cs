using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkUsers : MonoBehaviour
{
    // Start is called before the first frame update

    public  static int playerCount = 0;
    public  static bool p1Ready = false;
    public  static bool p2Ready = false;
    public  static bool p3Ready = false;
    public  static bool p4Ready = false;
    public GameObject ReadyButton;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(){
        if (playerCount < 4){
            playerCount ++;
        }
    }

    public void p1Click(){
        AddPlayer();
        p1Ready = true;
    }
    public void p2Click(){
        AddPlayer();
        p2Ready = true;
    }
    public void p3Click(){
        AddPlayer();
        p3Ready = true;
    }
    public void p4Click(){
        AddPlayer();
        p4Ready = true;
    }

    public void readyUp(){
        if (playerCount > 1){
            if (playerCount ==2){
                if (p1Ready == true && p2Ready == true){
                    ReadyButton.SetActive(true);
                }
            }
            
            else if (playerCount == 3){
                if (p1Ready == true && p2Ready == true && p3Ready == true){
                    ReadyButton.SetActive(true);
                }
            }
            else if (playerCount == 4){
                if (p1Ready == true && p2Ready == true  && p3Ready == true  && p4Ready == true){
                    ReadyButton.SetActive(true);
                }
            }
        }
    }
}
