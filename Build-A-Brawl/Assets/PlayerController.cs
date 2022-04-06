using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(RigidbodyController))]
public class PlayerController : MonoBehaviour
{

	/*start of Anna*/
	public PlayerConfiguration playerConfig;
	public PauseMenu pause;
	private Controlss theControls;

	//end of Anna 
	

    private CreatureStats statsRef;

	[SerializeField] private PhysicMaterial m_slidePhysicMaterial;
	[SerializeField] private Collider[] m_bodyColliders;

    [Header("Animators")]
	[SerializeField] private PhysicsIKRig[] m_rigs;

	[Header("Player Display Settings")]
	public Color playerColour;
	[SerializeField] private MeshRenderer[] m_meshRenderers;
	[SerializeField] private SkinnedMeshRenderer[] m_skinnedMeshRenderers;

	public void SetPlayerColour(Color colour)
    {
		playerColour = colour;
		for (int i = 0; i < m_meshRenderers.Length; i++)
			m_meshRenderers[i].material.SetColor("_PlayerColour", colour);
		
		for (int i = 0; i < m_skinnedMeshRenderers.Length; i++)
			m_skinnedMeshRenderers[i].material.SetColor("_PlayerColour", colour);
	}

	[Header("Player Movement Settings")]
	public float playerSpeed = 5.0f;
	public float jumpHeight = 1.0f;
    public float rotateSpeed = 1.0f;

    // States
    public enum State 
	{ 
		Idle,						// Player is not performing any actions (moving excluded)
		Attacking, Holding,			// Player is performing an attack action or a grapple action
		Held, Stunned, Dead			// Player is unable to perform actions
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
	

	// Input Variable Cache
	private Vector2 moveInput = Vector2.zero;
	private bool jumped = false;


	// Throwable Object Cache
	public ThrowableObject m_heldObject;


	#region MonoBehaviour Functions
	
	private void Awake()
	{
		//Anna start
		theControls = new Controlss();

		//Anna end
	}

	private void Start()
	{
		// This is for quick testing please remove this function call later
		//SetPlayerColour(playerColour);

		// Initialize state
		m_stateDictionary = new Dictionary<State, UnityAction>
		{
			{ State.Idle,    new UnityAction(HandleIdleState) },			
			{ State.Stunned, new UnityAction(HandleStunState) }
		};
		m_currentState = State.Idle;

		// get the components that have been added in through the character controller at the top
		m_controller = gameObject.GetComponent<RigidbodyController>();
		m_rigidbody = GetComponent<Rigidbody>();

		m_bodyColliders = GetComponents<Collider>();

        statsRef = transform.parent.GetComponent<CreatureStats>();

        forwardDir = transform.forward;

		m_controller.OnGrounded += () => m_controller.useFloat = true;
	}

	void Update()
	{
		// Checking what state the player is currently in
		if (m_currentState != State.Held && m_currentState != State.Stunned && m_currentState != State.Dead)
		{
			// If PhysicsIKRig is not in the animated state, do so
			foreach (PhysicsIKRig rig in m_rigs)
			{
				if (rig.currentState != PhysicsIKRig.State.Animated)
					rig.SetRagdoll(false);
			}

			Vector3 cameraRight = Camera.main.transform.right;
			Vector3 cameraCrossForward = Vector3.Cross(cameraRight, Vector3.up);
			Vector3 move = (cameraRight * moveInput.x + cameraCrossForward * moveInput.y).normalized;
			m_controller.SetVelocity(move * playerSpeed);

			//rotates player to move in the direction they are facing
			if (move != Vector3.zero)
				forwardDir = move;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forwardDir), 5 * rotateSpeed * Time.deltaTime);
		}
		else
        {
			// If PhysicsIKRig is in the animated state, activate ragdoll
			foreach (PhysicsIKRig rig in m_rigs)
			{
				if (rig.currentState != PhysicsIKRig.State.Ragdoll)
					rig.SetRagdoll(true);
			}
		}

		if (!m_controller.isGrounded)
			m_controller.useFloat = false;

		bounds = new Bounds();

		springConstant = attackSpringConstant;
		if (m_heldObject != null)
		{
			springConstant = grabSpringConstant;

			Bounds handBounds = fistLeftRigidbody.GetComponent<Collider>().bounds;
			handBounds.Encapsulate(fistRightRigidbody.GetComponent<Collider>().bounds);

			m_heldObject.rigidbody.rotation = transform.rotation;
			m_heldObject.rigidbody.position = transform.forward + handBounds.center;

			bounds = handBounds;
		}
	}
	Bounds bounds;

	private void FixedUpdate()
	{
		if (jumped && m_controller.isGrounded)
		{
			m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, jumpHeight, m_rigidbody.velocity.z);
			m_controller.useFloat = false;
			jumped = false;
		}

		HandleFistTransforms();
	}

	/*Anna start*/
	public void InitializePlayer(PlayerConfiguration config)
	{
		
		playerConfig = config;
		playerConfig.Input.onActionTriggered += Input_onActionTriggered;
		Debug.Log("Player initialized and configered");
	}

	private void Input_onActionTriggered(CallbackContext context)
	{
		if (context.action.name == theControls.Player.Move.name)
		{
			OnMove(context);
		} 
		
		if (context.action.name == theControls.Player.Jump.name)
		{
			OnJump(context);
		}

		if(context.action.name == theControls.Player.PickUp.name)
		{
			OnPickUp(context);
		}

		if(context.action.name == theControls.Player.RightPunch.name)
		{
			OnRightPunch(context);
		}

		if(context.action.name == theControls.Player.LeftPunch.name)
		{
			OnLeftPunch(context);
		}

		if(context.action.name == theControls.Player.PauseMenu.name)
		{
			OnPause(context);
		}

		if(context.action.name == theControls.Player.MakeNoise.name)
		{
			OnNoise(context);
		}
	}

	//Anna end

	#endregion



	#region Input Action Callback Functions

	public void OnMove(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		//triggered returns boolean true if triggered on the frame
		if (m_currentState == State.Stunned || m_currentState == State.Held || m_currentState == State.Dead)
			return;

		if (context.action.phase == InputActionPhase.Started)
			jumped = true;
	}

	public void OnPause(InputAction.CallbackContext context)
	{
		pause.Pause();
	}

	public void OnLeftPunch(InputAction.CallbackContext context) => attackPressed[LEFT_LIMB] = context.action.triggered;
	public void OnRightPunch(InputAction.CallbackContext context) => attackPressed[RIGHT_LIMB] = context.action.triggered;

	public void OnPickUp(InputAction.CallbackContext context)
	{
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

	public void OnNoise(InputAction.CallbackContext context)
	{

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

	private void HandleFistTransforms()
	{
		if (m_currentState != State.Stunned)
		{
			if (attackPressed[LEFT_LIMB] && isAttacking[LEFT_LIMB] == null)
			{
				isAttacking[LEFT_LIMB] = false ? // (statsRef.GetAttackTypeL() != BodyPartData.animType.Robot) ?
					StartCoroutine(Punch(fistLeftRigidbody, LEFT_LIMB, 0.7f)) :
					StartCoroutine(Punch(fistLeftRigidbody, LEFT_LIMB, 0.8f, 0.3f, false));
			}

			if (attackPressed[RIGHT_LIMB] && isAttacking[RIGHT_LIMB] == null)
			{
				isAttacking[RIGHT_LIMB] = false ? //(statsRef.GetAttackTypeR() != BodyPartData.animType.Robot) ?
					StartCoroutine(Punch(fistRightRigidbody, RIGHT_LIMB, 0.7f)) :
					StartCoroutine(Punch(fistRightRigidbody, RIGHT_LIMB, 0.8f, 0.3f, false));
			}
		}

		// Hook's law for spring physics
//		if (isAttacking[LEFT_LIMB] == null)
			HooksLaw(fistLeftRigidbody, anchorLeft);
//		if (isAttacking[RIGHT_LIMB] == null)
			HooksLaw(fistRightRigidbody, anchorRight);

		UpdateRotation(fistLeftRigidbody);
		UpdateRotation(fistRightRigidbody);
	}

	// A physics calculation to determine the force applied to a spring
	private void HooksLaw(Rigidbody rigidbody, Transform anchor)
	{
		Vector3 x = (rigidbody.position - anchor.position);		// Distance from equalibrium 
		Vector3 springForce = -springConstant * x;				// Hook's Law formula
		rigidbody.AddForce(springForce);
	}

	private void UpdateRotation(Rigidbody rigidbody)
    {
		Quaternion rot = Quaternion.LookRotation(transform.forward, Vector3.up);
		rigidbody.rotation = rot;
    }

	private IEnumerator Punch(Rigidbody rigidbody, int limb, float recoverTime, float activeTime = 0.0f, bool impulse = true)
	{
		SphereCollider collider = rigidbody.gameObject.GetComponent<SphereCollider>();
		collider.enabled = true;

		if (!impulse)
		{
			float activeAttackTime = 0.0f;
			while (activeAttackTime < activeTime)
			{
				rigidbody.AddForce(transform.forward * attackForce, ForceMode.Force);
				yield return null;
				activeAttackTime += Time.deltaTime;
			}
		}
		else
		{
			rigidbody.AddForce(transform.forward * attackForce, ForceMode.Impulse);
		}

		collider.enabled = false;
		yield return new WaitForSeconds(recoverTime);
		AttackComplete(limb);
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
		Collider[] colliders = Physics.OverlapBox(origin, grabBounds, transform.rotation);
		if (colliders.Length == 0)
			return;

		float closestDistance = Mathf.Infinity;
		foreach (Collider collider in colliders)
        {
			if (collider.transform.root != transform.root && collider.TryGetComponent(out ThrowableObject throwable))
            {
				float checkedDistance = Vector3.Distance(collider.transform.position, transform.position);
				if (checkedDistance < closestDistance)
                {
					m_heldObject = throwable;
					closestDistance = checkedDistance;
                }
            }
        }

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
			print($"{other.name} : {other.GetState()}");
		}
        m_heldObject.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        m_heldObject.SetGrabbed(true, transform);
		SetState(State.Holding);
	}

	public void HandleThrowAction()
	{
		PlayerController other = m_heldObject.GetComponent<PlayerController>();
		if (other != null)
		{
			other.SetState(State.Stunned);
		}

		Rigidbody rb = m_heldObject.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);

		m_heldObject.SetGrabbed(false);

		m_heldObject = null;
		isGrabbing = null;
		SetState(State.Idle);
	}

	private void HandleStunState()
    {
		m_controller.isStunned = true;
		for (int i = 0; i < m_bodyColliders.Length; i++)
			m_bodyColliders[i].material = null;

		m_controller.useFloat = false;
		m_controller.useBalance = false;
		m_controller.useMovement = false;

		float duration = 2.5f; // time in seconds
		if (isRecoveringFromStun == null)
			isRecoveringFromStun = StartCoroutine(RecoverFromStun(duration));
    }

	private void HandleIdleState()
	{
		m_controller.isStunned = false;
		for (int i = 0; i < m_bodyColliders.Length; i++)
			m_bodyColliders[i].material = m_slidePhysicMaterial;

		m_controller.ResetVelocity();
		
		m_controller.useFloat = true;
		m_controller.useMovement = true;
		m_controller.useBalance = true;
	}

	private Coroutine isRecoveringFromStun = null;
	private IEnumerator RecoverFromStun(float duration)
    {
		yield return new WaitForSeconds(duration);
		SetState(State.Idle);
		isRecoveringFromStun = null;
    }

	#endregion

    #endregion

    public void Stun(float duration)
    {
        //set state to stunned
        //waitforsecs
        //set state to idle
    }

#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;

		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

		Vector3 origin = grabOrigin + Vector3.forward * grabBounds.z / 2.0f;
		Gizmos.DrawWireCube(origin, grabBounds);

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(bounds.center, bounds.size);
	}
#endif
}