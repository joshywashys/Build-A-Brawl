using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]

public class MoveInputEvent : UnityEvent<float, float> { }

public class personTest : MonoBehaviour
{
    Controls playerControls;
    public MoveInputEvent moveInputEvent;
    Vector3 move;


    void Awake()
    {
        playerControls = new Controls();

        playerControls.Gameplay.Jump.performed += ctx => Jump();

    }

    void Jump()
    {
        transform.position += Vector3.up * 1.0f;
   
    }

    void OnEnable()
    {
        playerControls.Gameplay.Enable();
        playerControls.Gameplay.Move.performed += OnMovePerformed;
        playerControls.Gameplay.Move.canceled += OnMovePerformed;
    }

    void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        moveInputEvent.Invoke(moveInput.x, moveInput.y);
       // Debug.Log($"Player Controller: Move Input:{moveInput}");
    }

    void OnDisable()
    {
        playerControls.Gameplay.Disable();
    }
}
