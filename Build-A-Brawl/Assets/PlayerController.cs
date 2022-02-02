// Code referenced from https://docs.unity3d.com/ScriptReference/CharacterController.Move.html

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(RigidbodyController))]
public class PlayerController : MonoBehaviour
{
	// Serialize the numbers so they can be adjusted right from Unity.
	// General player characteristics are serialized so they can be adjusted in Unity

	[Header("Player Movement Settings")]
	[SerializeField] private float playerSpeed = 5.0f;
	[SerializeField] private float jumpHeight = 1.0f;
	
	//  [SerializeField] private BoolEvent onPickedUpLeft;
	//  [SerializeField]
	//  private float grabRange = 2.0f;

	// States
	public enum State 
	{ 
		Idle,									// Player is not performing any actions (moving excluded)
		Attacking,								// Player is performing an attack action
		Grabbing, Holding, Throwing,			// Player is performing a grapple action in the sequence
		Stunned, Held, Thrown, Dead             // Player is unable to perform actions
	}
	private State m_currentState;
	private Dictionary<State, UnityAction> m_stateDictionary;

	public void SetState(State newSate)
	{
		if (m_currentState == newSate)
			return;

		m_currentState = newSate;
		OnStateChanged(newSate);
	}
	public State GetState() { return m_currentState; }
	private void OnStateChanged(State state) 
	{
		if (m_stateDictionary.TryGetValue(state, out UnityAction callback))
			callback?.Invoke();
	}


	// Movement Variable Cache
	private RigidbodyController m_controller;
	private Rigidbody m_rigidbody;
	private Vector3 forwardDir;
	private bool groundedPlayer;
	

	// Input Variable Cache
	private Vector2 moveInput = Vector2.zero;
	private bool jumped = false;


	// Throwable Object Cache
	public ThrowableObject m_heldObject;


	#region MonoBehaviour Functions

	private void Start()
	{
		// Initialize state
		m_stateDictionary = new Dictionary<State, UnityAction>()
		{
		};
		
		m_currentState = State.Idle;

		// get the components that have been added in through the character controller at the top
		m_controller = gameObject.GetComponent<RigidbodyController>();
		m_rigidbody = GetComponent<Rigidbody>();

		forwardDir = transform.forward;

		m_controller.OnGrounded += () => m_controller.useFloat = true;
	}

	void Update()
	{
		// Checking what state the player is currently in
		if (!m_currentState.HasFlag(State.Held | State.Stunned | State.Thrown | State.Dead))
		{
			Vector3 cameraRight = Camera.main.transform.right;
			Vector3 cameraCrossForward = Vector3.Cross(cameraRight, Vector3.up);
			Vector3 move = (cameraRight * moveInput.x + cameraCrossForward * moveInput.y).normalized;
			m_controller.SetVelocity(move * playerSpeed);

			//rotates player to move in the direction they are facing
			if (move != Vector3.zero)
				forwardDir = move;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forwardDir), 5 * Time.deltaTime);
		}

		springConstant = attackSpringConstant;
		if (m_heldObject != null)
		{
			springConstant = grabSpringConstant;
			m_heldObject.rigidbody.position = (fistLeftRigidbody.position + fistRightRigidbody.position) / 2.0f;
		}
	}

	private void FixedUpdate()
	{
		if (jumped && m_controller.isGrounded)
		{
			m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, jumpHeight, m_rigidbody.velocity.z);
			m_controller.useFloat = false;
			jumped = false;
		}

		HandleFistLocations();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag(attackTag) &&
			   (collision.gameObject != fistLeftRigidbody.gameObject ||
				collision.gameObject != fistRightRigidbody.gameObject))
		{
			OnHit(collision);
		}
	}

	#endregion

	#region Input Action Callback Functions

	//triggered on usage of move control
	public void OnMove(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
	}

	//triggered on usage of jump control
	public void OnJump(InputAction.CallbackContext context)
	{
		//triggered returns boolean true if triggered on the frame
		if (context.action.phase == InputActionPhase.Started)
			jumped = true;
	}

	public void OnLeftPunch(InputAction.CallbackContext context) => attackPressed[LEFT_LIMB] = context.action.triggered;
	public void OnRightPunch(InputAction.CallbackContext context) => attackPressed[RIGHT_LIMB] = context.action.triggered;

	//triggered on usage of pickup control
	public void OnPickUp(InputAction.CallbackContext context)
	{
		// these special conditions will be created *after* the initial action works
		//if items are not in posession of left arm, pickup in left
		// else if items are not in posession of right arm, pickup in right
		// else emit UI of arms full

		// This doesn't need to be cached since this should be a state driven - Tristan
	
		if (context.action.triggered)
		{
			if (m_currentState == State.Holding)
			{
				HandleThrowAction();
				return;
			}
			
			HandleGrabbingAction();
		}
	}

	#endregion

	#region COMBAT SYSTEM

	private float springConstant;

	#region Attacking Logic

	// These variables should determined by the arm variants instead of this class
	[Header("Player Attack Debug Values")]
	public float attackSpringConstant = 80;
	public float attackForce = 30;

	// Used to for calculation and application of Hook's Law to make arms feel more spring-like
	[Header("Player Attack Settings")]
	public Rigidbody fistLeftRigidbody;
	public Rigidbody fistRightRigidbody;

	public Transform anchorLeft;
	public Transform anchorRight;

	// These variables are used to keep track of current action progress
	private const int LEFT_LIMB = 0, RIGHT_LIMB = 1;
	private bool[] attackPressed = { false, false };
	Coroutine[] isAttacking = { null, null };

	private void HandleFistLocations()
	{
		if (m_currentState != State.Stunned)
		{
			if (attackPressed[LEFT_LIMB] && isAttacking[LEFT_LIMB] == null)
				isAttacking[LEFT_LIMB] = StartCoroutine(Punch(fistLeftRigidbody, LEFT_LIMB, 0.7f));

			if (attackPressed[RIGHT_LIMB] && isAttacking[RIGHT_LIMB] == null)
				isAttacking[RIGHT_LIMB] = StartCoroutine(Punch(fistRightRigidbody, RIGHT_LIMB, 0.7f));
		}

		// Hook's law for spring physics
		HooksLaw(fistLeftRigidbody, anchorLeft);
		HooksLaw(fistRightRigidbody, anchorRight);
	}

	// A physics calculation to determine the force applied to a spring
	private void HooksLaw(Rigidbody rigidbody, Transform anchor)
	{
		Vector3 x = (rigidbody.transform.position - anchor.position);	// Distance form equalibrium 
		Vector3 springForce = -springConstant * x;						// Hook's Law formula
		rigidbody.AddForce(springForce);
	}

	private IEnumerator Punch(Rigidbody rigidbody, int limb, float activeTime)
	{
		SphereCollider collider = rigidbody.gameObject.GetComponent<SphereCollider>();
		collider.enabled = true;

		rigidbody.AddForce(transform.forward * attackForce, ForceMode.Impulse);
		yield return new WaitForSeconds(activeTime);

		AttackComplete(limb);
		
		collider.enabled = false;
	}
	private void AttackComplete(int limb)
	{
		isAttacking[limb] = null;
	}

	#endregion

	#region Grabbing Logic

	[Header("Player Grapple Settings")]
	public Vector3 grabBounds;
	public Vector3 grabOrigin;
	public float grabSpringConstant = 160.0f;
	public float throwForce = 15.0f;

	private Coroutine isGrabbing = null;
	private void HandleGrabbingAction()
	{
		// If the player is performing any action do not attempt to grab
		if (m_currentState != State.Idle || isGrabbing != null)
			return;

		// Check if a throwable object is in range
		Vector3 origin = transform.position + grabOrigin;
		Vector3 halfExtent = new Vector3(grabBounds.x, grabBounds.y, 1.0f);
		RaycastHit hit;
		if (!Physics.BoxCast(origin, halfExtent, transform.forward, out hit, transform.rotation, grabBounds.z))
			return;

		m_heldObject = hit.transform.GetComponent<ThrowableObject>();
		if (m_heldObject == null || m_heldObject.gameObject == gameObject)
			return;

		// Pick up object
		isGrabbing = StartCoroutine(OnGrabSuccessed());
		print(m_heldObject.name);
	}

	private IEnumerator OnGrabFailed()
	{
		yield return null;
	}

	private IEnumerator OnGrabSuccessed()
	{
		yield return null;

		PlayerController other = m_heldObject.GetComponent<PlayerController>();
		if (other != null)
		{
			other.SetState(State.Held);
		}

		m_heldObject.IsGrabbed(true);
		SetState(State.Holding);
	}

	public void HandleThrowAction()
	{
		PlayerController other = m_heldObject.GetComponent<PlayerController>();
		if (other != null)
		{
			other.SetState(State.Thrown);
		}

		Rigidbody rb = m_heldObject.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);

		m_heldObject.IsGrabbed(false);

		m_heldObject = null;
		isGrabbing = null;
		SetState(State.Idle);
	}

	#endregion

	[Header("Player Collision Tags")]
	public string attackTag = "heavyAttack";
	public void OnHit(Collision hit)
	{
		m_rigidbody.constraints = RigidbodyConstraints.None;

		Vector3 average = Vector3.zero;
		for (int i = 0; i < hit.contactCount; i++)
			average += hit.GetContact(i).point;
		average /= hit.contactCount;

		m_rigidbody.AddForceAtPosition(hit.impulse, average, ForceMode.Impulse);

		//SetState(State.Stunned);
	}

	#endregion

#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;

		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

		Vector3 origin = grabOrigin + Vector3.forward * grabBounds.z / 2.0f;
		Gizmos.DrawWireCube(origin, grabBounds);
	}
#endif
}