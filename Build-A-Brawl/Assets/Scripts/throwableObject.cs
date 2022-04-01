using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class ThrowableObject : MonoBehaviour
{
	[HideInInspector]
	public Transform holder;
	[HideInInspector]
	public new Rigidbody rigidbody;

	public new Collider collider;
	public Transform[] holdPoints;
	public UnityEvent onGrab;

	public bool IsGrabbed { get; private set; } = false;

	public void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		//holdPoints = new Transform[1];
		//holdPoints[1] = gameObject.transform;
	}

	public void SetGrabbed(bool pickedUp, Transform grabber = null)
	{
		collider.enabled = !pickedUp;
		IsGrabbed = pickedUp;

		if (pickedUp){
			holder = grabber;
		}

		onGrab?.Invoke();
	}

	internal bool IsHolder(Transform transform)
	{
		if (holder == null)
			return false;
		return holder.root == transform.root;
	}

#if UNITY_EDITOR
	/*
	[Header("Debug")]
	[SerializeField] private float gizmoSize = 0.25f;
	[SerializeField] private Color gizmoColour = Color.blue;
	//private void OnDrawGizmos()
	{
		Gizmos.color = gizmoColour;
        //if (holdPoints.Length == 0) { return; }
		//foreach (Transform point in holdPoints)
			//Gizmos.DrawWireSphere(point.position, gizmoSize);
	}
	*/
#endif
}
