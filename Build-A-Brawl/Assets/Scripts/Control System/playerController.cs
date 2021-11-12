using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//will go through and put comments on Nov 12th

public class playerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 280.0f;
    public float jumpVelocity = 5.0f;
    
    float horizontal;
    float vertical;

    private void Update()
    {
        Vector3 moveDirection = Vector3.forward * vertical + Vector3.right * horizontal;
        

        Vector3 proCamForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
        Quaternion rotationToCamera = Quaternion.LookRotation(proCamForward, Vector3.up);
        

        moveDirection = rotationToCamera * moveDirection;
        Quaternion rotationToMoveDirection = Quaternion.LookRotation(moveDirection, Vector3.up);



        if (moveDirection.magnitude > 0)
        {
            rotationToMoveDirection = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToMoveDirection, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump"))
        {
            moveDirection.y += Mathf.Sqrt(1.0f * -3.0f * -9.8f);
        }

        transform.position += moveDirection * speed * Time.deltaTime;
        
    }

    public void OnMoveInput(float horizontal, float vertical)
    {
        this.vertical = vertical;
        this.horizontal = horizontal;
        Debug.Log($"Player Controller: Move Input: {vertical}, {horizontal}");
    }

    public void OnJumpInput (float vertical)
    {
        this.vertical = vertical;
    }
}
