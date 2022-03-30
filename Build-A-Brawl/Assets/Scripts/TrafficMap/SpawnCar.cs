using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    public GameObject car1,car2,car3,car4,car5,car6, spawnLocation;
    // Start is called before the first frame update
    public int carColour = 1;
    void Start()
    {
        if (gameObject == GameObject.Find("nSpawn")){
            spawnLocation = GameObject.Find("nSpawn");
            InvokeRepeating("spawnCar", 5, 20);
        }
        else if (gameObject == GameObject.Find("sSpawn")){
            spawnLocation = GameObject.Find("sSpawn");
            InvokeRepeating("spawnCar", 10, 20);
        }
        else if (gameObject == GameObject.Find("wSpawn")){
            spawnLocation = GameObject.Find("wSpawn");
            InvokeRepeating("spawnCar", 15, 20);
        }
        else if (gameObject == GameObject.Find("eSpawn")){
            spawnLocation = GameObject.Find("eSpawn");
            InvokeRepeating("spawnCar", 20, 20);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnCar(){
        if (carColour ==1){
            Instantiate(car1, spawnLocation.transform.position, spawnLocation.transform.rotation);
            carColour++;
        }
        else if (carColour ==2){
            Instantiate(car2, spawnLocation.transform.position, spawnLocation.transform.rotation);
            carColour++;
        }
        else if (carColour ==3){
            Instantiate(car3, spawnLocation.transform.position, spawnLocation.transform.rotation);
            carColour++;
        }
        else if (carColour ==4){
            Instantiate(car4, spawnLocation.transform.position, spawnLocation.transform.rotation);
            carColour++;
        }
        else if (carColour ==5){
            Instantiate(car5, spawnLocation.transform.position, spawnLocation.transform.rotation);
            carColour++;
        }
        else if (carColour ==6){
            Instantiate(car6, spawnLocation.transform.position, spawnLocation.transform.rotation);
            carColour = 1;
        }
    }
}
