using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Arena", menuName = "Arena Data")]
public class ArenaData : ScriptableObject
{
	public string displayName;
	public Image displayThumbnail;
	public int buildIndex;
}