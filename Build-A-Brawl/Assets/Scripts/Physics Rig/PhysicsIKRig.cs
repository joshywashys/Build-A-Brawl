using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RigBuilder))]
public class PhysicsIKRig : MonoBehaviour
{
	private Animator m_animator;
	private Transform m_tip;
	private Transform m_target;

	private void InitializeTip()
    {
		if (rig.transform.TryGetComponent(out ChainIKConstraint chainIK))
		{
			chainIK.data.target = m_target;
			m_tip = chainIK.data.tip;
			return;
		}

		if (rig.transform.TryGetComponent(out TwoBoneIKConstraint twoBoneIK))
		{
			twoBoneIK.data.target = m_target;
			m_tip = twoBoneIK.data.tip;
			return;
		}

		// Debug.LogError($"[InitializeTip] : {name} was unable to obtain IK Constraint Data - Please add either a ChainIKConstraint or TwoBoneIKConstraint component to the gameObject");
	}
	private void InitializeTarget()
    {
		GameObject[] objects = GameObject.FindGameObjectsWithTag(m_targetTag);
		for (int i = 0; i < objects.Length; i++)
        {
			if (objects[i].transform.root == transform.root)
			{
				m_target = objects[i].transform;
				objects[i].tag = "heavyAttack";
				return;
			}
		}

		// Debug.LogError($"[InitializeTarget] : {name} was unable to find a gameObject with tag matching {m_targetTag} - was the tag misspelt?");
	}
	
	[Header("IK Rig Settings")]
	public Rig rig;
	[SerializeField] private string m_targetTag;

	[Header("Physics Rig Settings")]
	public Ragdoll ragdoll;
	
	public enum State { Animated, Ragdoll }
	public State currentState;

	private void Start()
    {
		m_animator = GetComponent<Animator>();

		InitializeTarget();
		InitializeTip();

		// As the Animation Rig currently stand,
		// the rigs won't work properly if data is modified in during runtime.
		// To fix this you'll have to rebuild the rig builder manually.
		GetComponent<RigBuilder>().Build();

		SetRagdoll(currentState == State.Ragdoll);
	}

    public void SetRagdoll(bool active)
    {
		ragdoll.SetActive(active);

		if (!active)
			m_target.position = m_tip.position;

		m_animator.enabled = !active;

		currentState = active ? State.Ragdoll : State.Animated;
	}
}

[System.Serializable]
public class Ragdoll
{
	[SerializeField] private Bone[] bones;

	public void SetActive(bool active)
    {
		if (!active)
			ResetVelocity();

		for (int i = 0; i < bones.Length; i++)
		{
			bones[i].rigidbody.constraints =  active ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
			//bones[i].collider.enabled = active;
		}
    }

	public void ResetVelocity()
    {
		for (int i = 0; i < bones.Length; i++)
        {
			bones[i].rigidbody.velocity = Vector3.zero;
			bones[i].rigidbody.angularVelocity = Vector3.zero;
        }
    }
}

[System.Serializable]
public class Bone
{
	public Rigidbody rigidbody;
	public Collider collider;
}
