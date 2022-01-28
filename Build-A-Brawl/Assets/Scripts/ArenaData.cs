using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Arena", menuName = "Arena Data")]
public class ArenaData : ScriptableObject
{
	[System.Serializable]
	public class SceneData
	{
		public bool useIndex = false;
		public int buildIndex = -1;
		public string sceneName = null;
	}

	[System.Serializable]
	public class TransitionData
    {
		public Vector3 inPosition;
		[Tooltip("A curve to animated the camera moving towards it's default position in the scene. Value percentage offset form the default (0.0f is default position - 1.0f is fully offset).")]
		public AnimationCurve animateIn = new AnimationCurve(new Keyframe(0, 1, 0, 0), new Keyframe(1, 0, -2, 2));

		public Vector3 outPosition;
		[Tooltip("A curve to animated the camera moving away from it's default position in the scene. Value percentage offset form the default (0.0f is default position - 1.0f is fully offset).")]
		public AnimationCurve animateOut = new AnimationCurve(new Keyframe(0, 0, -2, 2), new Keyframe(1, 1, 0, 0));
	}

	[Header("UI Info")]
	public string displayName;
	public Image displayThumbnail;

	[Header("SceneManager Info")]
	public TransitionData transition;
	public SceneData scene;
}