using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PhysicsIKRig : MonoBehaviour
{
	public bool usePhysics;
	[Range (0, 1)]
	public float blend;

	public Transform root;
	public Transform target;
	public Transform tip;
	public Transform[] bones;

	public Transform physicsAnchor;
	public Transform physicsTip;
	public Transform[] physicsBones;

	private Transform offset;
	private Vector3[] eulerAngleOffset;
	
	private TwoBoneIKConstraint ik;

	public Rigidbody[] boneRigidbodies;

	void Start()
	{
		offset = new GameObject($"offset_{bones[0].name}").transform;
		offset.position = bones[0].position;
		offset.parent = root;

		ik = GetComponentInChildren<TwoBoneIKConstraint>();

		int physicsBonesCount = physicsBones.Length;
		
		boneRigidbodies = new Rigidbody[physicsBonesCount + 1];
		eulerAngleOffset = new Vector3[physicsBonesCount];

		for (int i = 0; i < physicsBonesCount; i++)
        {
			boneRigidbodies[i] = physicsBones[i].GetComponent<Rigidbody>();
			eulerAngleOffset[i] = physicsBones[i].rotation.eulerAngles;
        }
		boneRigidbodies[physicsBonesCount] = physicsTip.GetComponent<Rigidbody>();
	}

	void LateUpdate()
	{
		BlendBones();
	}

	void BlendBones()
	{
		physicsAnchor.position = offset.position;
		physicsAnchor.rotation = offset.rotation;

		ik.weight = usePhysics ? 0 : 1;

		if (usePhysics)
		{
			for (int i = 0; i < boneRigidbodies.Length; i++)
				boneRigidbodies[i].useGravity = true;

			for (int i = 0; i < bones.Length; i++)
				SetTransforms(bones[i], physicsBones[i]);

			target.position = physicsBones[physicsBones.Length - 1].position;
			target.rotation = physicsBones[physicsBones.Length - 1].rotation;

			tip.position = physicsTip.position;
		}
		else
		{
			for (int i = 0; i < boneRigidbodies.Length; i++)
			{
				boneRigidbodies[i].useGravity = false;
				boneRigidbodies[i].velocity = Vector3.zero;
			}

			for (int i = 0; i < bones.Length; i++)
				SetTransforms(physicsBones[i], bones[i]);

			physicsTip.position = tip.position;
		}
	}

	(Vector3, Quaternion) LerpTransforms(Transform origin, Transform target, float blendFactor)
    {
		Vector3 pos = Vector3.Lerp(origin.position, target.position, blendFactor);
		Quaternion rot = Quaternion.Lerp(origin.rotation, target.rotation, blendFactor);

		return (pos, rot);
    }

	void SetTransforms(Transform set, Transform get)
	{
		set.position = get.position;
		set.rotation = get.rotation;
	}

#if UNITY_EDITOR
	[Header("Debug")]
	[SerializeField] private bool debugPreview = true;
	
	private void OnDrawGizmos()
    {
		if (!debugPreview)
			return;

		for (int i = 1; i < physicsBones.Length; i++)
			Gizmos.DrawLine(physicsBones[i - 1].position, physicsBones[i].position);
	}
#endif
}
