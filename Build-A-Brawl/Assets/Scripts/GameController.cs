using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
	public static GameController Instance { get; private set; }

	public Dictionary<int, PlayerGameData> players;
	public static PlayerGameData[] Players => Instance.players.Values.ToArray();

	public TextMeshProUGUI timerText;
	public float timerDuration;
	private float m_countDownTimer;
	
	public ArenaData[] m_arenas;

	// Initialization
	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);

		players = new Dictionary<int, PlayerGameData>();

		StartMatch();
	}

	// Player Join/Leave Logic
	public void OnPlayerJoined(PlayerInput player)
    {
		players.Add(player.playerIndex, new PlayerGameData(player.transform));
    }

	public void OnPlayerLeaves(PlayerInput player)
    {
		players.Remove(player.playerIndex);
    }

	public static PlayerGameData GetPlayer(int playerIndex) => Instance.players[playerIndex];
	
	// Match Game Logic
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

		// TO-DO: Display round results;	
		yield return LoadNextRandomArena();
	}

	public IEnumerator LoadNextRandomArena()
    {
		// Random Arena BuildIndex
		int nextSceneIndex = m_arenas[Random.Range(0, m_arenas.Length)].scene.buildIndex;
		
		// TO-DO: transition old scene out of view;
		
		AsyncOperation nextArena = SceneManager.LoadSceneAsync(nextSceneIndex);
		yield return nextArena;

		// TO-DO: transition new scene into view;
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

// Game Data ---- Custom Datatype
public class PlayerGameData
{
	public bool isAlive = true;
	public int matchScore = 0;
	public uint gameScore = 0;

	public Transform transform { get; private set; }
	public PlayerGameData(Transform transform)
    {
		this.transform = transform;
    }
}
