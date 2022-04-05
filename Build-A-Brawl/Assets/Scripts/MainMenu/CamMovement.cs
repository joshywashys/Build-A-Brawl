using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



    public class CamMovement : MonoBehaviour
    {
        public Camera camera;
        public GameObject targetNext;
        public float speed = 30.0f;
        public float Rotspeed = 3.0f;
        public int menuLayer = 1;
        public bool next, prev = false;
        public GameObject UIButtons1;
        [SerializeField] public bool inMenu;

        public bool nextLocation;

        public UnityEvent CameraLocation;

        [SerializeField] public GameObject targetLocation;

        // Start is called before the first frame update
        void Start()
        {
            UIButtons1 = GameObject.FindWithTag("UI1");
            menuLayer = 1;
            menuLayer = 1;
            nextLocation = false;
            UIButtons1.gameObject.SetActive(true);
            inMenu = true;
        }

        public void cameraMovement(GameObject location)
        {
            //Debug.Log("Boom");
            targetLocation = location;
            nextLocation = true;
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