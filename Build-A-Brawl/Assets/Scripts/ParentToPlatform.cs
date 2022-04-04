using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ParentToPlatform : MonoBehaviour
{
	public Transform rootPlatform;

	private Dictionary<GameObject, Transform> m_originalParent = new Dictionary<GameObject, Transform>();

	private void OnTriggerStay(Collider other)
	{
		if (other.TryGetComponent(out RigidbodyController controller))
        {
			if (!m_originalParent.ContainsKey(other.gameObject))
				m_originalParent.Add(other.gameObject, other.transform.parent);

			if (!controller.isGrounded)
			{
				controller.transform.parent = m_originalParent[other.gameObject];
				return;
			}

			controller.transform.parent = rootPlatform;
        }
	}

    private void OnTriggerExit(Collider other)
    {
		if (m_originalParent.ContainsKey(other.gameObject))
        {
			other.transform.parent = m_originalParent[other.gameObject];
			return;
        }
    }

#if UNITY_EDITOR
	private enum DebugVisualType { None, Outline, Fill };

	[Header("Debug Visualization")]
	[SerializeField] private DebugVisualType displayZoneType;
	[SerializeField] private Color displayZoneColour = Color.blue;

    private void OnDrawGizmos()
    {
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

		Gizmos.color = displayZoneColour;
		switch(displayZoneType)
        {
			case DebugVisualType.Outline:
				Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
				break;
			case DebugVisualType.Fill:
				Gizmos.DrawCube(Vector3.zero, Vector3.one);
				break;
        }
    }
#endif
}
