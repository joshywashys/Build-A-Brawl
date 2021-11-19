using UnityEngine;
using UnityEngine.UI;

public class CreaturePreviewUI : MonoBehaviour
{
	public const byte MAX_PLAYER_COUNT = 4;

	//public GameObject creatorSetPrefab;
	
	public Camera previewRenderCamera;
	public RawImage previewDisplay;
	public Canvas canvas;

	private RenderTexture[] m_renderTextures = new RenderTexture[MAX_PLAYER_COUNT];
	private byte m_activePlayerCount;

	private void Update()
    {
		UpdateDisplayPreview();
	}

	public void UpdateDisplayPreview()
    {
		int width = Mathf.Clamp((int)(previewDisplay.rectTransform.rect.width * canvas.scaleFactor), 1, Screen.width);
		int height = Mathf.Clamp((int)(previewDisplay.rectTransform.rect.height * canvas.scaleFactor), 1, Screen.height);


		if (m_renderTextures[0])
			m_renderTextures[0].Release();
		m_renderTextures[0] = new RenderTexture(width, height, 24);
		previewRenderCamera.targetTexture = m_renderTextures[0];
		previewRenderCamera.Render();
		previewDisplay.texture = m_renderTextures[0];
	}
}
