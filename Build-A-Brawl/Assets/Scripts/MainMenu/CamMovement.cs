using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



    public class CamMovement : MonoBehaviour
    {
        public Camera camera;
        [SerializeField] public GameObject mainCamera;
        public GameObject targetNext;
        public float speed = 30.0f;
        public float Rotspeed = 3.0f;
        public int menuLayer = 1;
        public bool next, prev = false;
        public GameObject UIButtons1;

        public bool nextLocation;

        public UnityEvent CameraLocation;

        public GameObject location1;
        public GameObject location2;
        public GameObject location3;
        public GameObject location4;
        public GameObject location5;
        public GameObject location6;



    [SerializeField] public GameObject targetLocation;

        // Start is called before the first frame update
        void Start()
        {
            UIButtons1 = GameObject.FindWithTag("UI1");
            menuLayer = 1;
            menuLayer = 1;
            nextLocation = false;
            UIButtons1.gameObject.SetActive(true);
        }

        public void cameraMovement(GameObject location)
        {
            //Debug.Log("Boom");
            targetLocation = location;
            nextLocation = true;
        }
      //main is 2
      //maps is 3
      //controls is 5
      //cc is 4

        public void backMove()
        {

        Debug.Log("This has been called");
            if (mainCamera.transform.position == location5.transform.position) {

            cameraMovement(location2);

            } else if (mainCamera.transform.position == location4.transform.position) {

            cameraMovement(location3);
        } else if (mainCamera.transform.position == location3.transform.position) {

            cameraMovement(location2);
        }
                else 
            {
                return;
            }

    }


        // Update is called once per frame
        void Update()
        {

            if(nextLocation == true)
            {
                float step = speed * Time.deltaTime;
                float stepRot = Rotspeed * Time.deltaTime;
                camera.transform.position = Vector3.MoveTowards(camera.transform.position, targetLocation.transform.position, step);
                camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetLocation.transform.rotation, stepRot);
                
            } else
            {
            return;
            }


             
        }
    }