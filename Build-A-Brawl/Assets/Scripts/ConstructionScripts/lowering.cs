using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lowering : MonoBehaviour
{
    public float enemySpeed;

    GameObject nextPos;

    public int nextPosIndex;
    
    void Start()
    {
        nextPosIndex = 1;
        nextPos = GameObject.Find("Vpoint"  + nextPosIndex);
    }

    // Update is called once per frame
    void Update()
    {
        moveEnemy();
    }

    void moveEnemy() 
    {
        if (transform.position == nextPos.transform.position){
            if (nextPosIndex == 2){
                nextPosIndex--;
            }
            else if (nextPosIndex == 1){
                nextPosIndex++;
            }
            nextPos = GameObject.Find("Vpoint" + nextPosIndex);
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, nextPos.transform.position, enemySpeed*Time.deltaTime);
        }
    }
}
