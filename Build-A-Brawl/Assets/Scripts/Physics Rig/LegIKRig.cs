using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RigBuilder))]
public class LegIKRig : MonoBehaviour
{
	private Animator m_animator;
	private RigidbodyController m_rController;
	
	private float m_animSpeed = 0;
	private float m_animSpeedDampVel = 0;

	public LayerMask groundLayer;

	[System.Serializable]
	public class StrideData
	{
		public float length = 1.0f;
		public AnimationCurve lengthCurve = new AnimationCurve(new Keyframe(0, 1));
		[Space]
		public float height = 1.0f;
		public AnimationCurve heightCurve = new AnimationCurve(new Keyframe(0, 1));
		[Space]
		public float waddle = 1.0f;
		public AnimationCurve waddleCurve = new AnimationCurve(new Keyframe(0, 1));
		[Space]
		public float speed = 2.0f;
		public AnimationCurve speedCurve = new AnimationCurve(new Keyframe(0, 1));
	}
	[Header("Procedural Walking Settings")]
	public StrideData stride;

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

		private float legSide = 0;

		public void Initialize(float leg, LayerMask layer)
        {
			legSide = leg;
			m_layerMask = layer;
			m_raycastDistance = raycastOrigin.position.y - foot.position.y + targetOffset + 0.2f;
		}

		public void SetIKTarget(Vector3 forwards, StrideData stride, float timeOffset, float runSpeed)
        {
			Ray ray = new Ray(raycastOrigin.position, Vector3.down);
			if (Physics.Raycast(ray, out RaycastHit hit, m_raycastDistance, m_layerMask))
			{
				m_weightInflence = 1.0f;
				m_weightLerpSpeed = 15.0f;

				Vector3 hitTarget = hit.point + hit.normal * targetOffset;

				// Procedural Walking code
				float time = Time.time * stride.speed + timeOffset;

				float lengthTime = Mathf.Sin(time);
				Vector3 forwardPos = forwards * lengthTime * stride.length * stride.lengthCurve.Evaluate(lengthTime);

				float heightTime = Mathf.Cos(time);
				Vector3 upwardPos = Vector3.up * heightTime * stride.height * stride.heightCurve.Evaluate(heightTime);

				float waddleTime = Mathf.Cos(time);
				Vector3 sidePos = legSide * Vector3.Cross(forwards, Vector3.up) * waddleTime * stride.waddle * stride.waddleCurve.Evaluate(waddleTime);

				m_targetPosition = ((forwardPos + upwardPos + sidePos) * runSpeed) + hitTarget;
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

		leftLeg.Initialize(1.0f, groundLayer);
		rightLeg.Initialize(-1.0f, groundLayer);

		SetRagdoll(currentState == State.Ragdoll);
    }

    private void Update()
	{
		// For testing purposes
		if (m_rController != null)
		{
			float targetAnimSpeed = Mathf.Clamp01(m_rController.isGrounded ? m_rController.Velocity : 0.0f);
			m_animSpeed = Mathf.SmoothDamp(m_animSpeed, targetAnimSpeed, ref m_animSpeedDampVel, Time.deltaTime);
		}
		else
		{
			m_animSpeed = 1.0f;
		}

		SetLeftLegIKTarget(m_animSpeed);
		SetRightLegIKTarget(m_animSpeed);

		leftLeg.UpdateWeightInflence();
		rightLeg.UpdateWeightInflence();
        
		leftLeg.UpdateIKTargetPosition();
		rightLeg.UpdateIKTargetPosition();

		//leftLeg.UpdateIKTargetRotation();
		//rightLeg.UpdateIKTargetRotation();
	}

	public void SetLeftLegIKTarget(float speed)
    {
		leftLeg.SetIKTarget(transform.forward, stride, 0.0f, speed);
    }

	public void SetRightLegIKTarget(float speed)
    {
		rightLeg.SetIKTarget(transform.forward, stride, 3.0f, speed);
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

		if (leftLeg.raycastOrigin == null && leftLeg.target == null && leftLeg.foot == null &&
			rightLeg.raycastOrigin == null && rightLeg.target == null && rightLeg.foot == null)
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
