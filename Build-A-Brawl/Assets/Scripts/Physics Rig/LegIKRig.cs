using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RigBuilder))]
public class LegIKRig : MonoBehaviour
{
	public bool overrideIKWeights = false;
	public LayerMask ignoreLayer;

	private Animator m_animator;

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
		private Vector3 m_targetPosDampVel;
		private float m_weightInflence;
		private float m_weightLerpSpeed;
		private float m_raycastDistance;

		public void Initialize(LayerMask layer)
        {
			m_layerMask = layer;
			m_raycastDistance = raycastOrigin.position.y - foot.position.y + targetOffset + 0.2f;
        }

		public void SetIKTarget()
        {
			// Should probably include a max distance
			Ray ray = new Ray(raycastOrigin.position, Vector3.down);
			if (Physics.Raycast(ray, out RaycastHit hit, m_raycastDistance, m_layerMask))
			{
				m_weightInflence = 1.0f;
				m_weightLerpSpeed = 15.0f;

				m_targetPosition = hit.point + hit.normal * targetOffset;
				return;
			}

			m_weightInflence = 0.0f;
			m_weightLerpSpeed = 5.0f;
		}

		public void UpdateIKTargetPosition()
        {
			target.position = Vector3.SmoothDamp(target.position, m_targetPosition, ref m_targetPosDampVel, Time.deltaTime);
        }

		public void UpdateWeightInflence()
        {
			rig.weight = Mathf.Lerp(rig.weight, m_weightInflence, Time.deltaTime * m_weightLerpSpeed);
        }
	}
	public Leg leftLeg, rightLeg;

    private void Start()
    {
		m_animator = GetComponent<Animator>();

		leftLeg.Initialize(~ignoreLayer);
		rightLeg.Initialize(~ignoreLayer);

		SetRagdoll(currentState == State.Ragdoll);
    }

    private void Update()
	{
		if (overrideIKWeights)
        {
			SetLeftLegIKTarget();
			SetRightLegIKTarget();

			leftLeg.UpdateWeightInflence();
			rightLeg.UpdateWeightInflence();
        }

		leftLeg.UpdateIKTargetPosition();
		rightLeg.UpdateIKTargetPosition();
	}

	public void SetLeftLegIKTarget()
    {
		leftLeg.SetIKTarget();
    }

	public void SetRightLegIKTarget()
    {
		rightLeg.SetIKTarget();
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
