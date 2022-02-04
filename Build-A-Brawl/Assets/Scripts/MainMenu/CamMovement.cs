using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public Camera camera;
    public GameObject targetNext;
    public float speed = 30.0f;
    public float Rotspeed = 3.0f;
    public int menuLayer = 1;
    public bool next, prev = false;
    public GameObject UIButtons1;
    // Start is called before the first frame update
    void Start()
    {
        UIButtons1 = GameObject.FindWithTag("UI1");
        menuLayer = 1;
        UIButtons1.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (next == true){
            float step = speed * Time.deltaTime;
            float stepRot = Rotspeed * Time.deltaTime;
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, targetNext.transform.position, step);
            camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetNext.transform.rotation, stepRot);
            
        }
        else if (prev == true){
            float step = speed * Time.deltaTime;
            float stepRot = Rotspeed * Time.deltaTime;
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, targetNext.transform.position, step);
            camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetNext.transform.rotation, stepRot);
            
        }
        
        if (Input.GetKeyDown(KeyCode.Return) && menuLayer < 6){
            Debug.Log("press register");
            
            targetNext = GameObject.Find("location"+(menuLayer+1));
            prev = false;
            next = true;
            menuLayer ++;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menuLayer > 1){
            Debug.Log("press register");
            
            targetNext = GameObject.Find("location"+(menuLayer-1));
            next = false;
            prev = true;
            menuLayer--;
        }
        else{
            return;
        }

        
        
    }
}
