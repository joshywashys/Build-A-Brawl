using UnityEngine;

public class CombatController : MonoBehaviour
{
	public float groundedThrowAngle = -30.0f;
	public float aerialThrowAngle = 80.0f;
	
	public float[] m_attackChargeTime = { 3.0f, 3.0f };
	public float[] m_attackChangePercentage = { 0.0f, 0.0f };

	private bool isAttacking = false;

	public void ChargeAttack(int limb)
	{
		m_attackChangePercentage[limb] = Mathf.Clamp01(m_attackChangePercentage[limb] + Time.fixedDeltaTime / m_attackChargeTime[limb]);
	}

	public void Attack()
	{
		
	}

	private AnimationCurve powerCurve;
	private float attackRange;
	public void Punch(float charge)
    {
		
	}

#if UNITY_EDITOR
	[Header("Debug Visualizers")]
	[SerializeField] private bool m_showThrowAngles = true;
	
	private void OnDrawGizmos()
	{
		if (m_showThrowAngles)
			DrawThrowAngleGizmos();
	}

	private void DrawThrowAngleGizmos()
	{
		Vector3 aerialVector = (Mathf.Cos(aerialThrowAngle * Mathf.Deg2Rad) * transform.forward + Mathf.Sin(aerialThrowAngle * Mathf.Deg2Rad) * Vector3.up).normalized;
		Vector3 groundedVector = (Mathf.Cos(groundedThrowAngle * Mathf.Deg2Rad) * transform.forward + Mathf.Sin(groundedThrowAngle * Mathf.Deg2Rad) * Vector3.up).normalized;
		
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(transform.position, transform.position + aerialVector * 3.0f);

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + groundedVector * 3.0f);
	}
#endif
}
