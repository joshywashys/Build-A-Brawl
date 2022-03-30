using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrowableObject : MonoBehaviour
{
	[HideInInspector]
	public Transform holder;
	[HideInInspector]
	public new Rigidbody rigidbody;

	public new Collider collider;
	public Transform[] holdPoints;

	public bool IsGrabbed { get; private set; } = false;

	public void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public void SetGrabbed(bool pickedUp, Transform grabber = null)
	{
		collider.enabled = !pickedUp;
		IsGrabbed = pickedUp;

		if (pickedUp)
			holder = grabber;
	}

	internal bool IsHolder(Transform transform)
	{
		if (holder == null)
			return false;
		return holder.root == transform.root;
	}

#if UNITY_EDITOR
	[Header("Debug")]
	[SerializeField] private float gizmoSize = 0.25f;
	[SerializeField] private Color gizmoColour = Color.blue;
	private void OnDrawGizmos()
	{
		Gizmos.color = gizmoColour;
		foreach (Transform point in holdPoints)
			Gizmos.DrawWireSphere(point.position, gizmoSize);
	}
#endif
}
