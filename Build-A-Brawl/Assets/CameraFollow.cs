using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public GameObject[] players;

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

		if (players.Length == 0){
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
		var bounds = new Bounds(players[0].transform.position, Vector3.zero);
		for (int i = 0; i < players.Length; i++)
		{
			//if (players[i].isAlive)
				bounds.Encapsulate(players[i].transform.position);
		}

		return bounds.size.z;
	}

	Vector3 getCenter() 
	{
		if (players.Length == 1) 
		{
			return players[0].transform.position;
		}

		var bounds = new Bounds(players[0].transform.position, Vector3.zero);
		for (int i = 0; i < players.Length; i++)
		{
			//if (players[i].isAlive)
				bounds.Encapsulate(players[i].transform.position);
		}
        Vector3 centerPoint = bounds.center;
        centerPoint.x = 0;
		return centerPoint;
	}
}
