using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float moveVelocity = 5.0f;
    public float rotationSpeed = 280.0f;

    float horizontal;
    float vertical;

    private void Update()
    {
        Vector3 moveDirection = Vector3.forward * vertical + Vector3.right * horizontal;

        Quaternion rotationToMoveDirection = Quaternion.LookRotation(moveDirection, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToMoveDirection, rotationSpeed * Time.deltaTime);
        

        if (moveDirection.magnitude > 0)
        {
            rotationToMoveDirection = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToMoveDirection, rotationSpeed * Time.deltaTime);
        }

        transform.position += moveDirection * moveVelocity * Time.deltaTime;

    }

    public void OnMoveInput(float horizontal, float vertical)
    {
        this.vertical = vertical;
        this.horizontal = horizontal;
        Debug.Log($"Player Controller: Move Input: {vertical}, {horizontal}");
    }
}
