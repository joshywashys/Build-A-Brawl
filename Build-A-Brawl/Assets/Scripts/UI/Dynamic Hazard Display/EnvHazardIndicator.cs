using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;

[RequireComponent(typeof(Camera))]
public class EnvHazardIndicator : MonoBehaviour
{
	public static EnvHazardIndicator instance;

	public Canvas canvas;
	public GameObject hazardIndicatorPrefab;
	public AnimationCurve curve;

	private Camera m_camera;
	private Pool<GameObject> hazardPool;
	private Dictionary<GameObject, Coroutine> m_displaying = new Dictionary<GameObject, Coroutine>();

	void Awake()
	{
		if (instance != null)
			return;

		instance = this;
		m_camera = GetComponent<Camera>();

		hazardPool = new Pool<GameObject>(
			(GameObject go)	=> { return go.activeSelf; },
			()				=> { return Instantiate(hazardIndicatorPrefab, canvas.transform); }
		);
	}

	public void OnDisplayHazard(GameObject hazardPoint)
	{
		m_displaying.Add(hazardPoint, StartCoroutine(DisplayHazardIndicator(hazardPoint.transform.position)));
	}

	private IEnumerator DisplayHazardIndicator(Vector3 hazardWorldPoint)
	{
		RectTransform hazard = hazardPool.GetAvailable().GetComponent<RectTransform>();
		float time = 0;

		float frequency = curve.keys[curve.length - 1].time;
		while (true)
        {
			Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(m_camera, hazardWorldPoint);
			screenPos.x = Mathf.Clamp(screenPos.x, 0.0f, Screen.width) - Screen.width / 2.0f;
			screenPos.y = Mathf.Clamp(screenPos.y, 0.0f, Screen.height) - Screen.height / 2.0f;

			hazard.localPosition = screenPos;
			hazard.localScale = Vector3.one * curve.Evaluate(time % frequency);
			
			time += Time.deltaTime;
			yield return null;
		}
	}

	public void OnHideHazard(GameObject hazardWorldPoint)
    {
		Coroutine coroutine = null;
		if (!m_displaying.TryGetValue(hazardWorldPoint.gameObject, out coroutine) || coroutine == null)
			return;

		StopCoroutine(coroutine);
		m_displaying.Remove(hazardWorldPoint.gameObject);
    }
}
