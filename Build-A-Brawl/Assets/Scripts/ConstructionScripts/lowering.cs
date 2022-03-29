using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lowering : MonoBehaviour
{
    public float mass;

    public Rigidbody ball;
    
    void Start()
    {
        mass = 1;
        InvokeRepeating("massChange", 1.0f, 40f);
    }

    void massChange()
    {
        ball.mass = mass;
        mass = mass + 10;
    }
}
