using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



    public class TheEnd : MonoBehaviour
    {
        public Camera camera;
        // public GameObject targetNext;
        public float speed = 30.0f;
        public float Rotspeed = 3.0f;
        // public bool next, prev = false;
        // public GameObject UIButtons1;

        //public bool nextLocation;

        // public UnityEvent CameraLocation;

        [SerializeField] public GameObject location1;
        [SerializeField] public GameObject location2;
        public int menuGo = 0;

        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating("zoomUp", 15.0f, 10f);
        }

        // public void cameraMovement(GameObject location)
        // {
        //     //Debug.Log("Boom");
        //     targetLocation = location;
        //     nextLocation = true;
        // }

        void zoomUp()
        {
                menuGo++;
        }

        // Update is called once per frame
        void Update()
        {

            if(menuGo == 0)
            {
                float step = speed * Time.deltaTime;
                float stepRot = Rotspeed * Time.deltaTime;
                camera.transform.position = Vector3.MoveTowards(camera.transform.position, location1.transform.position, step);
                camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, location1.transform.rotation, stepRot);
                
            } else if (menuGo == 1){
                float step = speed * Time.deltaTime;
                float stepRot = Rotspeed * Time.deltaTime;
                camera.transform.position = Vector3.MoveTowards(camera.transform.position, location2.transform.position, step);
                camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, location2.transform.rotation, stepRot);
            } else if (menuGo > 2){
                Debug.Log("fade to black - go to menu");
            }
            


             
        }
    }