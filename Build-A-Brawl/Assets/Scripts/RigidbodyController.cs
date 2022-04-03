using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(CapsuleCollider))]
public class RigidbodyController : MonoBehaviour
{
    private CreatureStats statsRef;

    [SerializeField] private LayerMask ignoreLayer;
	public bool isStunned = false;

	[Header("Floating Settings")]
	public bool useFloat = true;
	public float floatHeight;
	public float m_floatSpringStrength;
	[SerializeField] private float m_floatSpringDamper;
	[SerializeField] private Vector3 m_rayOriginOffset { get { return Vector3.up * m_rayOffset; } }

	[Header("Balancing")]
	public bool useBalance = true;
	public float m_balanceSpringStrength;
	public float m_balanceSpringDamper;

	[Header("Ground Check Settings")]
	[SerializeField] private float m_groundCheckBuffer;
	[SerializeField] private float m_rayOffset;
	public bool isGrounded { get; private set; } = false;
	public UnityAction OnGrounded;

	[Header("Locomotion")]
	public bool useMovement = true;
	[SerializeField] private float m_acceleration;
	[SerializeField] private AnimationCurve m_accelerationFactor;
	[SerializeField] private float m_maxAccelForce;
	[SerializeField] private Vector3 m_forceScale;
	[SerializeField] private float m_gravityScale;

	[HideInInspector] public Vector3 groundNormal;

	private Rigidbody m_rigidbody;
	private Vector3 m_currentVel;
	private Vector3 m_rayDir = Vector3.down;
	private Ray m_ray;

	private Vector3 m_desiredVel;
    
    public void SetVelocity(Vector3 velocity) => m_desiredVel = velocity;
 
	public float Velocity { get { return m_rigidbody.velocity.magnitude; } }

    private void InitializeValues()
    {
        floatHeight = 3;
        m_floatSpringStrength = 30;
        m_floatSpringDamper = 4;
        
        m_balanceSpringStrength = 50;
        m_balanceSpringDamper = 1.5f;

        m_groundCheckBuffer = 0.75f;

        m_acceleration = 200;
        //m_accelerationFactor = 
        m_maxAccelForce = 150;
        m_forceScale = new Vector3(1, 0, 1);
        m_gravityScale = 5f;
    }

	private void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody>();
        statsRef = transform.parent.GetComponent<CreatureStats>();
        //InitializeValues();
    }

	private void FixedUpdate()
	{
		if (isStunned)
			return;

		m_ray = new Ray(transform.position + m_rayOriginOffset, m_rayDir);

		PhysicsCheck();

		Float();
		Balance();
		Move();
	}

	public void ResetVelocity()
    {
		m_rigidbody.velocity = Vector3.zero;
		m_rigidbody.angularVelocity = Vector3.zero;
    }

	private void PhysicsCheck()
	{
		if (Physics.Raycast(m_ray, out RaycastHit hit, floatHeight + m_groundCheckBuffer + m_rayOffset, ~ignoreLayer))
		{
			if (!isGrounded)
				OnGrounded?.Invoke();
			
			isGrounded = true;
			groundNormal = hit.normal;
			return;
		}

		isGrounded = false;
		groundNormal = Vector3.up;
		m_rigidbody.AddForce(Physics.gravity * m_gravityScale);
	}

	private void Float()
	{
		if (!useFloat)
			return;

		if (Physics.Raycast(m_ray, out RaycastHit hit, floatHeight * 2.0f + m_rayOffset, ~ignoreLayer))
		{
            Vector3 otherVel = Vector3.zero;
			Rigidbody otherBody = hit.rigidbody;
			if (otherBody != null)
				otherVel = otherBody.velocity;

			float rayDotVel = Vector3.Dot(m_rayDir, m_rigidbody.velocity);
			float otherDotVel = Vector3.Dot(m_rayDir, otherVel);

			float relVel = rayDotVel - otherDotVel;

			float x = hit.distance - floatHeight;
			float springForce = (x * m_floatSpringStrength) - (relVel * m_floatSpringDamper);
			m_rigidbody.AddForce(m_rayDir * springForce);

			if (otherBody != null)
				otherBody.AddForceAtPosition(m_rayDir * -springForce, hit.point);

		}
	}

	private void Move()
	{
		if (!useMovement)
			return;

		float velDot = Vector3.Dot(m_desiredVel.normalized, m_rigidbody.velocity.normalized);
		float accel = m_acceleration * m_accelerationFactor.Evaluate(velDot);

		m_currentVel = Vector3.MoveTowards(m_currentVel, m_desiredVel, accel * Time.fixedDeltaTime);
		Vector3 desiredAccel = (m_currentVel - m_rigidbody.velocity) / Time.fixedDeltaTime;

		float maxAccel = m_maxAccelForce * m_accelerationFactor.Evaluate(velDot);
		desiredAccel = Vector3.ClampMagnitude(desiredAccel, maxAccel);

		m_rigidbody.AddForce(Vector3.Scale(desiredAccel * m_rigidbody.mass, m_forceScale));
	}

	private void Balance()
    {
		if (!useBalance)
			return;

		Quaternion neededRot = Quaternion.FromToRotation(transform.up, Vector3.up);
		
		neededRot.ToAngleAxis(out float angle, out Vector3 axis);

		axis.Normalize();
		float radAngle = angle * Mathf.Deg2Rad;

		m_rigidbody.AddTorque((axis * (radAngle * m_balanceSpringStrength)) - (m_rigidbody.angularVelocity * m_balanceSpringDamper));
	}

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
		Vector3 rayOrigin = transform.position + m_rayOriginOffset;

		Gizmos.color = Color.black;
		Gizmos.DrawLine(rayOrigin, rayOrigin + m_rayDir * (floatHeight * 2.0f + m_rayOriginOffset.y));

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(rayOrigin, rayOrigin + m_rayDir * (floatHeight + m_groundCheckBuffer + m_rayOriginOffset.y));

		Gizmos.color = isGrounded ? Color.green : Color.red;
		Gizmos.DrawLine(rayOrigin, rayOrigin + m_rayDir * (floatHeight + m_rayOriginOffset.y));
	}
#endif
}
