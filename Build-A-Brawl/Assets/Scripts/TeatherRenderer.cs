using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TeatherRenderer : MonoBehaviour
{
	private LineRenderer m_line;

	public Transform start;
	public Transform end;

	private void Start()
	{
		m_line = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		m_line.SetPositions(new Vector3[] { start.position, end.position });
	}
}
