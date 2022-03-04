using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carTravel : MonoBehaviour
{

    [SerializeField] GameObject redLight;
    [SerializeField] GameObject yellowLight;
    [SerializeField] GameObject greenLight;
    [SerializeField] GameObject destination;


    public Transform startDest;
    public Transform midDest;
    public Transform endDest;

    public bool destReached = false;
    public bool pastLine = false;

    public float speed = 30.0f;

    private float startTime;

    private float journeyLength;

    private void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startDest.position, midDest.position);
    }

    void Update()
    {
        // if light yellow, destLine not reached and headed West
        // wait until green, then go

        // if light green or yellow, destLine reached, and headed West, continue

        float equation = speed * Time.deltaTime;
        float limit = 73.2f;

        if (!destReached && this.transform.position.z != midDest.transform.position.z) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, midDest.transform.position, equation);
        } else if (this.transform.position.z == midDest.transform.position.z)
        {
            destReached = true;
        } else if (this.transform.position.z < limit)
        {
            pastLine = true;
        }

        if (!redLight.activeInHierarchy && destReached)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, endDest.transform.position, equation);
            Debug.Log("SECOND");
        }
        else if (pastLine)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, endDest.transform.position, equation);

        }

    }
}
