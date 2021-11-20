using UnityEngine;
using UnityEngine.Events;

public abstract class HazardObject : MonoBehaviour
{
	public GameObject hazardLocator;

	public UnityAction<GameObject> onDisplayHazard;
	public UnityAction<GameObject> onHideHazard;

	protected virtual void Start()
	{
		onDisplayHazard += EnvHazardIndicator.instance.OnDisplayHazard;
		onHideHazard += EnvHazardIndicator.instance.OnHideHazard;
	}
}
