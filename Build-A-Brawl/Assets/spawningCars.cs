using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//in this script, colour of car is randomly selected, car is spawned every few seconds, direction is chosen, and then car is destroyed.

public class spawningCars : MonoBehaviour
{
    [SerializeField] GameObject vRed;
    [SerializeField] GameObject vBlue;
    [SerializeField] GameObject vGreen;
    [SerializeField] GameObject vWhite;
    [SerializeField] GameObject vPink;
    [SerializeField] GameObject vBarf;

    GameObject vehicle;

    int numColour;
    int numPosition;
    List<GameObject> car = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        car.Add(vRed.gameObject);
        car.Add(vBlue.gameObject);
        car.Add(vGreen.gameObject);
        car.Add(vWhite.gameObject);
        car.Add(vPink.gameObject);
        car.Add(vBarf.gameObject);
        
        InvokeRepeating("spawnCar", 2.0f, 15f);
    }

    int randColor()
    {
        numColour = Random.Range(0, 5);
        Debug.Log("COLOUR CHOSEN");
        return numColour;
    }

    int randPosition()
    {
        numPosition = Random.Range(0, 4);
        Debug.Log("Position found");
        return numPosition;
    }

    void spawnCar()
    {
        
        numColour = randColor();
        Debug.Log("SPAWNED");
        GameObject.Instantiate(car[numColour]);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
