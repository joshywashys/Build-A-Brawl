using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
HOW/WHEN TO USE:
Make sure to only use Head/Torso/Arms/Legs child classes.
These scripts generate data for the creature creator to coordinate the parts together, and contain stats and modifiers for each part's gameplay.
Only attach those scripts to an object once:
-the collider's dimensions are correct
-the 3d model size corresponds to the collider's size
*/
public class BodyPart : MonoBehaviour
{
    //NOTE: width should always represent the X axis (wingspan). Z is depth.
    private float height;
    private float width;

    //these are in Awake so that they are calculated without even being in the scene
    void Awake()
    {
        height = GetComponent<Collider>().bounds.size.y;
        width = GetComponent<Collider>().bounds.size.x;
    }

    public float GetHeight()
    {
        return height;
    }

    public float GetWidth()
    {
        return width;
    }

    void Update()
    {
        
    }
}