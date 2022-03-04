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
	public static PlayerGameData GetPlayer(int playerIndex) => Instance.players[playerIndex];

	public TextMeshProUGUI timerText;
	public float timerDuration;
	private float m_countDownTimer;
	
	public ArenaData[] arenas;
	private ArenaData m_currentArena;

	private PlayerInputManager pim;

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

		pim = GetComponent<PlayerInputManager>();
		pim.onPlayerJoined += OnPlayerJoined;
		pim.onPlayerLeft += OnPlayerLeft;

		//StartMatch();

		m_currentArena = arenas[0];
	}

	Coroutine loadNextSceneRoutine = null;
    private void Start()
    {
		//if (loadNextSceneRoutine == null)
		//	loadNextSceneRoutine = StartCoroutine(LoadNextRandomArena());
	}

    #region Player Data Logic

    public void OnPlayerJoined(PlayerInput player)
	{
		players.Add(player.playerIndex, new PlayerGameData(player.transform));
	}

	public void OnPlayerLeft(PlayerInput player)
	{
		players.Remove(player.playerIndex);
	}
    
	#endregion

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

	Coroutine currentMatch = null;
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

	Coroutine transition = null;
	public IEnumerator LoadNextRandomArena()
	{
		if (transition != null)
			StopCoroutine(transition);

		// transition old scene out of view
		ArenaData.TransitionData currTransition = m_currentArena.transition;
		
		transition = StartCoroutine(TransitionCamera(Camera.main.transform.position, currTransition.outPosition, currTransition.animateOut));
		yield return transition;
		
		// load new random scene
		ArenaData nextArena = arenas[Random.Range(0, arenas.Length)];
		AsyncOperation nextScene = nextArena.scene.useIndex ? 
				SceneManager.LoadSceneAsync(nextArena.scene.buildIndex) :
				SceneManager.LoadSceneAsync(nextArena.scene.sceneName);
		yield return nextScene;

		// transition new scene into view
		StopCoroutine(transition);
		
		ArenaData.TransitionData nextTransition = nextArena.transition;
		transition = StartCoroutine(TransitionCamera(nextTransition.defaultPosition, nextTransition.inPosition, nextTransition.animateIn));
		yield return transition;

		m_currentArena = nextArena;

		print("Done");
	}

	public IEnumerator TransitionCamera(Vector3 start, Vector3 end, AnimationCurve transition)
    {
		float t = 0;
		while (t < 1.0f)
        {
			Camera.main.transform.position = Vector3.Lerp(start, end, transition.Evaluate(t));
			yield return null;
			t += Time.deltaTime;
		}
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
