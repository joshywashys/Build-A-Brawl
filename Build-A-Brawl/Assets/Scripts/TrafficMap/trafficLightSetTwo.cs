using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// change the traffic lights
// yellow for 10 seconds, red for 10 seconds, green for 10 seconds
public class trafficLightSetTwo : MonoBehaviour
{

    [SerializeField] public GameObject redLight;
    [SerializeField] public GameObject yellowLight;
    [SerializeField] public GameObject greenLight;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ywChangeLight", 5.0f, 30.0f);


    }

    // Update is called once per frame
    void rdChangeLight()
    {
        if (redLight.activeInHierarchy)
        {
            greenLight.SetActive(true);
            redLight.SetActive(false);
            InvokeRepeating("grChangeLight", 10.0f, 15.0f);
        }
    }

    void grChangeLight()
    {
        if (greenLight.activeInHierarchy)
        {
            greenLight.SetActive(false);
            yellowLight.SetActive(true);
        }
    }

    void ywChangeLight()
    {
        if (yellowLight.activeInHierarchy)
        {
            yellowLight.SetActive(false);
            redLight.SetActive(true);
            InvokeRepeating("rdChangeLight", 20.0f, 10.0f);

        }
    }
}
