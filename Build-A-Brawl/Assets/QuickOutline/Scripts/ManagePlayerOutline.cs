using UnityEngine;

public class ManagePlayerOutline : MonoBehaviour
{
	public GameObject[] rendererGameObjects;

	private PlayerOutline[] m_playerOutlines;

	public void AddOutine()
	{
		m_playerOutlines = new PlayerOutline[rendererGameObjects.Length];

		for (int i = 0; i < rendererGameObjects.Length; i++)
		{
			if (rendererGameObjects[i].TryGetComponent(out PlayerOutline outline))
            {
				m_playerOutlines[i] = outline;
				continue;
            }
				
			m_playerOutlines[i] = rendererGameObjects[i].AddComponent<PlayerOutline>();
		}
	}

	public void SetColour(Color colour)
    {
		for (int i = 0; i < m_playerOutlines.Length; i++)
			m_playerOutlines[i].OutlineColor = colour;
	}

	public void EnableOutline(bool active)
    {
		for (int i = 0; i < m_playerOutlines.Length; i++)
			m_playerOutlines[i].enabled = active;
    }
}