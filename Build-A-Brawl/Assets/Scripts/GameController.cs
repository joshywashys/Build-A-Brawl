using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
	public static GameController Instance { get; private set; }
	
	public TextMeshProUGUI timerText;
	public float timerDuration;

	private float m_countDownTimer;
	private float m_currentSceneIndex;

	private ArenaData[] m_arenas;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);

		StartMatch();
	}

	public void StartMatch()
	{
		if (currentMatch != null)
		{
			StopCoroutine(currentMatch);
			currentMatch = null;
		}

		currentMatch = StartCoroutine(CountDown());
	}

	Coroutine currentMatch;
	private IEnumerator CountDown()
	{
		m_countDownTimer = timerDuration;
		while (m_countDownTimer > 0)
		{
			UpdateTimer();
			yield return null;
			m_countDownTimer -= Time.deltaTime;
		}

		// TO-DO: Display round results and fade out;	
		yield return LoadNextRandomArena();
	}

	public IEnumerator LoadNextRandomArena()
    {
		// Random Arena BuildIndex
		int nextSceneIndex = m_arenas[Random.Range(0, m_arenas.Length)].buildIndex;
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		
		AsyncOperation nextArena = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
		yield return nextArena;

		SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));

		AsyncOperation prevArena = SceneManager.UnloadSceneAsync(currentSceneIndex);
		yield return prevArena;
	}

	private void UpdateTimer()
	{
		int minute = (int)(m_countDownTimer / 60.0f);
		int second = (int)(m_countDownTimer % 60.0f);

		string s_minute = (minute < 10) ? $"0{minute}" : minute.ToString();
		string s_second = (second < 10) ? $"0{second}" : second.ToString();
		timerText.SetText(s_minute + ":" + s_second);
	}
}

