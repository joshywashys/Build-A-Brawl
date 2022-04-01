using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RigBuilder))]
public class LegIKRig : MonoBehaviour
{
	private Animator m_animator;
	private RigidbodyController m_rController;
	private Vector4 m_stride { get { return new Vector4(0, strideHeight, strideLength, strideSpeed); } }

	public LayerMask groundLayer;

	[Header("Procedural Walking Settings")]
	public float strideLength = 1.0f;
	public float strideHeight = 1.0f;
	public float strideSpeed = 2.0f;

	[Header("Physics Rig Settings")]
	public Ragdoll ragdoll;

	public enum State { Animated, Ragdoll }
	public State currentState;

	[System.Serializable]
	public class Leg
	{
		public Rig rig;
		public Transform raycastOrigin;
		public float targetOffset;
		public Transform target;
		public Transform foot;

		private LayerMask m_layerMask;

		private Vector3 m_targetPosition;
		private Quaternion m_targetRotation;

		private Vector3 m_targetPosDampVel;

		private float m_weightInflence;
		private float m_weightLerpSpeed;
		private float m_raycastDistance;

		public void Initialize(LayerMask layer)
        {
			m_layerMask = layer;
			m_raycastDistance = raycastOrigin.position.y - foot.position.y + targetOffset + 0.2f;
        }

		public void SetIKTarget(Vector3 forwards, Vector4 stride, float timeOffset, float runSpeed)
        {
			Ray ray = new Ray(raycastOrigin.position, Vector3.down);
			if (Physics.Raycast(ray, out RaycastHit hit, m_raycastDistance, m_layerMask))
			{
				m_weightInflence = 1.0f;
				m_weightLerpSpeed = 15.0f;

				Vector3 hitTarget = hit.point + hit.normal * targetOffset;
				
				// Procedural Walking code
				Vector3 forwardPos = forwards * Mathf.Sin(Time.time * stride.w + timeOffset) * stride.z;
				Vector3 upwardPos = Vector3.up * Mathf.Clamp01(Mathf.Cos(Time.time * stride.w + timeOffset)) * stride.y;
				Vector3 sidePos = Vector3.Cross(forwards, Vector3.up) * Mathf.Clamp01(Mathf.Cos(Time.time * stride.w + timeOffset)) * stride.x;

				m_targetPosition = ((forwardPos + upwardPos) * runSpeed) + hitTarget;
				return;
			}

			m_weightInflence = 0.0f;
			m_weightLerpSpeed = 5.0f;
		}

		public void UpdateIKTargetPosition()
        {
			target.position = Vector3.SmoothDamp(target.position, m_targetPosition, ref m_targetPosDampVel, Time.deltaTime);
        }

		public void UpdateIKTargetRotation()
        {
			target.rotation = m_targetRotation;
        }

		public void UpdateWeightInflence(float multiplier = 1.0f)
        {
			rig.weight = Mathf.Lerp(rig.weight, m_weightInflence, Time.deltaTime * m_weightLerpSpeed) * multiplier;
        }
	}
	public Leg leftLeg, rightLeg;

    private void Start()
    {
		m_animator = GetComponent<Animator>();
		m_rController = GetComponentInParent<RigidbodyController>();

		leftLeg.Initialize(groundLayer);
		rightLeg.Initialize(groundLayer);

		SetRagdoll(currentState == State.Ragdoll);
    }

    private void Update()
	{
		//float leftLegWeight = m_animator.GetFloat("LeftIKWeight");
		//float rightLegWeight = m_animator.GetFloat("RightIKWeight");

		float animSpeed = Mathf.Clamp01(m_rController.isGrounded ? m_rController.Velocity : 0.0f);

		SetLeftLegIKTarget(animSpeed);
		SetRightLegIKTarget(animSpeed);

		leftLeg.UpdateWeightInflence();
		rightLeg.UpdateWeightInflence();
        
		leftLeg.UpdateIKTargetPosition();
		rightLeg.UpdateIKTargetPosition();

		//leftLeg.UpdateIKTargetRotation();
		//rightLeg.UpdateIKTargetRotation();

		//m_animator.SetFloat("Speed", animSpeed);
	}

	public void SetLeftLegIKTarget(float speed)
    {
		leftLeg.SetIKTarget(transform.forward, m_stride, 0.0f, speed);
    }

	public void SetRightLegIKTarget(float speed)
    {
		rightLeg.SetIKTarget(transform.forward, m_stride, 3.0f, speed);
    }

	public void SetRagdoll(bool active)
	{
		ragdoll.SetActive(active);

		if (!active)
		{
			leftLeg.target.position = leftLeg.foot.position;
			rightLeg.target.position = rightLeg.foot.position;
		}

		m_animator.enabled = !active;

		currentState = active ? State.Ragdoll : State.Animated;
	}

    private void OnDrawGizmos()
    {
		if (Application.isPlaying)
			return;

		// Left Leg
		Gizmos.color = Color.blue;
		Vector3 leftTargetOffset = leftLeg.target.position + Vector3.down * leftLeg.targetOffset;
		Gizmos.DrawWireSphere(leftTargetOffset, 0.5f);

		Gizmos.color = Color.black;
		float leftRaycastDistance = leftLeg.raycastOrigin.position.y - leftLeg.foot.position.y + leftLeg.targetOffset + 0.2f;
		Vector3 leftRaycastEnd = leftLeg.raycastOrigin.position + Vector3.down * leftRaycastDistance;
		Gizmos.DrawLine(leftLeg.raycastOrigin.position, leftRaycastEnd);

		// Right Leg
		Gizmos.color = Color.blue;
		Vector3 rightTargetOffset = rightLeg.target.position + Vector3.down * rightLeg.targetOffset;
		Gizmos.DrawWireSphere(rightTargetOffset, 0.5f);

		Gizmos.color = Color.black;
		float rightRaycastDistance = rightLeg.raycastOrigin.position.y - rightLeg.foot.position.y + rightLeg.targetOffset + 0.2f;
		Vector3 rightRaycastEnd = rightLeg.raycastOrigin.position + Vector3.down * rightRaycastDistance;
		Gizmos.DrawLine(rightLeg.raycastOrigin.position, rightRaycastEnd);
	}
}
