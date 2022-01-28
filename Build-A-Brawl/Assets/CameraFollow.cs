using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
	public Vector3 offset;
	public float smoothTime = 0.5f;
	public float minZoom = 40f;
	public float maxZoom = 10f;
	public float zoomLimit = 0f;

	private Vector3 velocity;
	private Camera cam;

	void Start() {
		cam = GetComponent<Camera>();
	}

	void LateUpdate() {

		if (GameController.Players.Length == 0){
			return;
		}

		Move();
		Zoom();
	}

	void Zoom() {
		float newZoom = Mathf.Lerp(maxZoom, minZoom, getDistance() / zoomLimit);
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
	}

	void Move() 
	{
		Vector3 center = getCenter();
		Vector3 newPos = center + offset;
		transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
	}

	float getDistance()
	{
		var bounds = new Bounds(GameController.Players[0].transform.position, Vector3.zero);
		for (int i = 0; i < GameController.Players.Length; i++)
		{
			if (GameController.Players[i].isAlive)
				bounds.Encapsulate(GameController.Players[i].transform.position);
		}

		return bounds.size.x;
	}

	Vector3 getCenter() 
	{
		if (GameController.Players.Length == 1) 
		{
			return GameController.Players[0].transform.position;
		}

		var bounds = new Bounds(GameController.Players[0].transform.position, Vector3.zero);
		for (int i = 0; i < GameController.Players.Length; i++)
		{
			if (GameController.Players[i].isAlive)
				bounds.Encapsulate(GameController.Players[i].transform.position);
		}

		return bounds.center;
	}
}
