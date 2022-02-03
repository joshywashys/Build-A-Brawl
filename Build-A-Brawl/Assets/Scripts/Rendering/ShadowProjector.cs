using UnityEngine;

[ExecuteInEditMode]
public class ShadowProjector : MonoBehaviour
{
	public Transform shadowTransform;

	[Header("Shadow Settings")]
	public float scale = 3.0f;

	[Header("Projection Settings")]
	public float maxDistance = 1000.0f;
	public float bufferDistance = 5.0f;
	
    private void Update()
	{
		CastShadow();
	}

	private void CastShadow()
    {
		float shadowDistance = maxDistance;
		
		Ray ray = new Ray(transform.position, Vector3.down);
		if (Physics.Raycast(ray, out RaycastHit hit))
        {
			shadowDistance = hit.distance;
		}

		shadowDistance += bufferDistance;

		Vector3 shadowPoint = new Vector3(transform.position.x, transform.position.y - shadowDistance * 0.5f, transform.position.z);
		Vector3 shadowScale = new Vector3(scale, shadowDistance, scale);

		shadowTransform.position = shadowPoint;
		shadowTransform.localScale = shadowScale;
	}
}