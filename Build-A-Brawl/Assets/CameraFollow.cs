using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private List<GameObject> players;

	public Vector3 offset;
	public float smoothTime = 1f;
	public float minZoom = 40f;
	public float maxZoom = 10f;
	public float zoomLimit = 0f;

	private Vector3 velocity;
	private Camera cam;

	void Zoom() {
		float newZoom = Mathf.Lerp(maxZoom, minZoom, getDistance() / zoomLimit);
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
	}

	void Move() 
	{
		Vector3 center = getCenter();
		Vector3 newPos = center + offset + new Vector3(4, 4, 0);
		transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
	}

	float getDistance()
	{
		var bounds = new Bounds(players[0].transform.position, Vector3.zero);
		for (int i = 0; i < players.Count; i++)
		{
			//if (players[i].isAlive)
				bounds.Encapsulate(players[i].transform.position);
		}

		return bounds.size.z;
	}

	Vector3 getCenter() 
	{
		if (players.Count == 1) 
		{
			return players[0].transform.position;
		}

		var bounds = new Bounds(players[0].transform.position, Vector3.zero);
		for (int i = 0; i < players.Count; i++)
		{
			//if (players[i].isAlive)
				bounds.Encapsulate(players[i].transform.position);
		}
        Vector3 centerPoint = bounds.center;
        centerPoint.x = 0;
		return centerPoint;
	}

    #region Monobehaviour Functions

    void LateUpdate()
    {
        if (players.Count == 0)
        {
            //print("no players");
            return;
        }

        Move();
        Zoom();
    }

    void Start()
    {
        cam = GetComponent<Camera>();

        // Get players
        players = new List<GameObject>();
        CreatureStats[] searchList = FindObjectsOfType<CreatureStats>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < searchList.Length; j++)
            {
                if (searchList[j].GetPlayerNum() == i + 1)
                {
                    players.Add(searchList[j].transform.parent.gameObject);
                }
            }
        }
    }

    #endregion
}
