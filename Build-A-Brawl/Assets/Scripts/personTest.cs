using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class personTest : MonoBehaviour
{
    Controls playerControls;

    Vector3 move;


    void Awake()
    {
        playerControls = new Controls();

        playerControls.Gameplay.Jump.performed += ctx => Jump();

        playerControls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector3>();
        playerControls.Gameplay.Move.canceled += ctx => move = Vector3.zero;
    }

    void Jump()
    {
        transform.position += Vector3.up * 1.0f;
   
    }

    private void Update()
    {
        Vector3 moving = new Vector3(move.x, 0, move.z) * Time.deltaTime;
        transform.Translate(move, Space.World);
        
    }

    void OnEnable()
    {
        playerControls.Gameplay.Enable();

    }

    void OnDisable()
    {
        playerControls.Gameplay.Disable();
    }
}
