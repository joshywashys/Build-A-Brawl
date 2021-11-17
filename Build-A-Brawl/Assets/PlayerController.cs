// Code referenced from https://docs.unity3d.com/ScriptReference/CharacterController.Move.html

using UnityEngine;
using UnityEngine.InputSystem;

//Adds in a character controller
[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    // Serialize the numbers so they can be adjusted right from Unity.

    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField] 
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Vector2 moveInput = Vector2.zero;
    private bool jumped = false;

    private void Start()
    {
        // get the components that have been added in through the character controller at the top
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //triggered returns boolean true if triggered on the frame
        jumped = context.action.triggered;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        //rotates player
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (jumped && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}