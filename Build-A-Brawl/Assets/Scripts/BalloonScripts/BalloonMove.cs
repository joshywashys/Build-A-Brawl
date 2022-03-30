using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMove : MonoBehaviour
{
    public float bMovespeed;

    GameObject nextPos;

    public int nextPosIndex;

    public int movementObj;
    
    void Start()
    {
        if (gameObject == GameObject.Find("balloon1")){
            movementObj = 1;
        } else if (gameObject == GameObject.Find("balloon2")){
            movementObj = 2;
        } else if (gameObject == GameObject.Find("balloon3")){
            movementObj = 3;
        } else if (gameObject == GameObject.Find("background")){
            movementObj = 4;
        }
        nextPosIndex = 1;
        nextPos = GameObject.Find("b"  + movementObj + "" + nextPosIndex);
    }

    // Update is called once per frame
    void Update()
    {
        moveEnemy();
    }

    void moveEnemy() 
    {
        if (transform.position == nextPos.transform.position){
            if (nextPosIndex == 4){
                nextPosIndex = 1;
            }
            else if (nextPosIndex < 4){
                nextPosIndex++;
            }
            nextPos = GameObject.Find("b" + movementObj + "" + nextPosIndex);
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, nextPos.transform.position, bMovespeed*Time.deltaTime);
        }
    }
}
