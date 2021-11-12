using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]


//classes declared
public class MoveInputEvent : UnityEvent<float, float> { }
public class JumpInputEvent : UnityEvent<float, float> { }
public class personTest : MonoBehaviour
{
    Controls playerControls;
    
    //declare instance of the classes
    public MoveInputEvent moveInputEvent;
    public JumpInputEvent jumpInputEvent;

    Vector3 move;


    void Awake()
    {
        //Set up class controls
        playerControls = new Controls();
    }

    

    // When created, register performed actions here
    void OnEnable()
    {
        
        playerControls.Gameplay.Enable();
        playerControls.Gameplay.Move.performed += OnMovePerformed;
        playerControls.Gameplay.Move.canceled += OnMovePerformed;
        playerControls.Gameplay.Jump.performed += OnJumpPerformed;
        playerControls.Gameplay.Jump.canceled += OnJumpPerformed;
    }

    // movement action function
    void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        moveInputEvent.Invoke(moveInput.x, moveInput.y);
       // Debug.Log($"Player Controller: Move Input:{moveInput}");
    }

    // jump action function
    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        Vector2 jumpInput = context.ReadValue<Vector2>();
        jumpInputEvent.Invoke(jumpInput.x, jumpInput.y);
        transform.position += Vector3.up * 1.0f;

    }

    void OnDisable()
    {
        playerControls.Gameplay.Disable();
    }
}
