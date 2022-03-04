using UnityEngine;

public class HitBoxController : MonoBehaviour
{
	public enum ColliderType { Collision, Trigger }

	public ColliderType colliderType; 
	public Collider[] colliders;
	
	public void EnableHitbox(bool active)
	{
		foreach (Collider collider in colliders)
        {
			bool useTrigger = active ? colliderType == ColliderType.Trigger : false;
			collider.isTrigger = useTrigger;
			collider.enabled = active;
		}
	}
}
