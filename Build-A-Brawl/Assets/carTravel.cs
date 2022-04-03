using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carTravel : MonoBehaviour
{

    [SerializeField] GameObject redLight;
    [SerializeField] GameObject yellowLight;
    [SerializeField] GameObject greenLight;
    [SerializeField] GameObject destination;
    [SerializeField] GameObject vehicle;

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
        for (int i = 1; i < 7; i++){
            if (gameObject == GameObject.Find("nVehic" + i + "(Clone)")){
                redLight = GameObject.Find("South").transform.Find("sRed").gameObject;
                yellowLight = GameObject.Find("South").transform.Find("sYellow").gameObject;
                greenLight = GameObject.Find("South").transform.Find("sGreen").gameObject;
                startDest = GameObject.Find("nSpawn").transform;
                midDest = GameObject.Find("nMid").transform;
                endDest = GameObject.Find("nEnd").transform;
            } else if (gameObject == GameObject.Find("sVehic" + i + "(Clone)")){
                redLight = GameObject.Find("North").transform.Find("nRed").gameObject;
                yellowLight = GameObject.Find("North").transform.Find("nYellow").gameObject;
                greenLight = GameObject.Find("North").transform.Find("nGreen").gameObject;
                startDest = GameObject.Find("sSpawn").transform;
                midDest = GameObject.Find("sMid").transform;
                endDest = GameObject.Find("sEnd").transform;
            } else if (gameObject == GameObject.Find("wVehic" + i + "(Clone)")){
                redLight = GameObject.Find("East").transform.Find("eRed").gameObject;
                yellowLight = GameObject.Find("East").transform.Find("eYellow").gameObject;
                greenLight = GameObject.Find("East").transform.Find("eGreen").gameObject;
                startDest = GameObject.Find("eSpawn").transform;
                midDest = GameObject.Find("wMid").transform;
                endDest = GameObject.Find("wEnd").transform;
            } else if (gameObject == GameObject.Find("eVehic" + i + "(Clone)")){
                redLight = GameObject.Find("West").transform.Find("wRed").gameObject;
                yellowLight = GameObject.Find("West").transform.Find("wYellow").gameObject;
                greenLight = GameObject.Find("West").transform.Find("wGreen").gameObject;
                startDest = GameObject.Find("eSpawn").transform;
                midDest = GameObject.Find("eMid").transform;
                endDest = GameObject.Find("eEnd").transform;
            } 
        }
        startTime = Time.time;
        journeyLength = Vector3.Distance(startDest.position, midDest.position);
    }

    void Update()
    {

        for (int i = 1; i < 7; i++){
            if (gameObject == GameObject.Find("nVehic" + i + "(Clone)")){
                if (redLight == null){
                    redLight = GameObject.Find("sRed");
                }
                if (yellowLight == null){
                    yellowLight = GameObject.Find("sYellow");
                }
                if (greenLight == null) {
                    greenLight = GameObject.Find("sGreen");
                }
                startDest = GameObject.Find("nSpawn").transform;
                midDest = GameObject.Find("nMid").transform;
                endDest = GameObject.Find("nEnd").transform;
            } else if (gameObject == GameObject.Find("sVehic" + i + "(Clone)")){
                if (redLight == null){
                    redLight = GameObject.Find("nRed");
                }
                if (yellowLight == null){
                    yellowLight = GameObject.Find("nYellow");
                }
                if (greenLight == null) {
                    greenLight = GameObject.Find("nGreen");
                }
                startDest = GameObject.Find("sSpawn").transform;
                midDest = GameObject.Find("sMid").transform;
                endDest = GameObject.Find("sEnd").transform;
            } else if (gameObject == GameObject.Find("wVehic" + i + "(Clone)")){
                if (redLight == null){
                    redLight = GameObject.Find("eRed");
                } 
                if (yellowLight == null){
                    yellowLight = GameObject.Find("nYellow");
                }
                if (greenLight == null) {
                    greenLight = GameObject.Find("eGreen");
                }
                startDest = GameObject.Find("eSpawn").transform;
                midDest = GameObject.Find("wMid").transform;
                endDest = GameObject.Find("wEnd").transform;
            } else if (gameObject == GameObject.Find("eVehic" + i + "(Clone)")){
                if (redLight == null){
                    redLight = GameObject.Find("wRed");
                }
                if (yellowLight == null){
                    yellowLight = GameObject.Find("wYellow");
                }
                if (greenLight == null) {
                    greenLight = GameObject.Find("wGreen");
                }
                startDest = GameObject.Find("eSpawn").transform;
                midDest = GameObject.Find("eMid").transform;
                endDest = GameObject.Find("eEnd").transform;
            } 
        }
        // if light yellow, destLine not reached and headed West
        // wait until green, then go

        // if light green or yellow, destLine reached, and headed West, continue

        float equation = speed * Time.deltaTime;
        float limit = 73.2f;

        isCollidable = false;

        if (!destReached && this.transform.position.z != midDest.transform.position.z) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, midDest.transform.position, equation);
        } else if (this.transform.position.z == midDest.transform.position.z)
        {
            destReached = true;
        } else if (this.transform.position.z < limit)
        {
            pastLine = true;
        }
        if (redLight != null){
            if ((!redLight.activeInHierarchy && destReached) || pastLine)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, endDest.transform.position, equation);
                isCollidable = true;
            }
        }
        Destroy(vehicle, 30);

    }

    private bool isCollidable = true;
    private void OnCollisionEnter(Collision collision)
    {
        if (!isCollidable)
            return;

        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            Rigidbody rig = GetComponent<Rigidbody>();

            Vector3 collisionPoint = Vector3.zero;
            foreach (ContactPoint contact in collision.contacts)
                collisionPoint += contact.point;
            collisionPoint /= collision.contactCount;

            controller.SetState(PlayerController.State.Stunned);

            rig.AddExplosionForce(200.0f, collisionPoint, 5.0f);;
        }
    }
}
