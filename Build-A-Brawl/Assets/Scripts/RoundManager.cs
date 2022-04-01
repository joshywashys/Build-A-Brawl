using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public BetaStageSelect stages;
    public List<string> selectedMaps;
    public int numRounds = 5;
    private int currRound = 0;
    private float time;

    public struct playerStats
    {
        decimal score;
        bool isAlive;
    }

    public int numPlayers;
    public List<playerStats> stats;

    #region Main Menu
    [Header("Main Menu")]
    public float waitTime;

    public UnityEvent onCancelStart;

    private bool starting = false;
    private IEnumerator crDropBlocks;
    private IEnumerator crCountdown;

    #region Blocks
    [Header("Main Menu Blocks")]
    public List<GameObject> numberBlocks;
    public Transform blockSpawnpoint;
    public bool useRandomRotation;
    public bool destroyBlocksOnCancel;
    public Vector3 launchVector;

    public IEnumerator DropBlocks()
    {
        for (int i = 0; i < numberBlocks.Count; i++)
        {
            Quaternion qt = Quaternion.identity;
            if (useRandomRotation) { qt = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)); }

            GameObject newBlock = Instantiate(numberBlocks[i], blockSpawnpoint.transform.position, qt, blockSpawnpoint);
            if (newBlock.GetComponent<NumberBlock>() == null) { newBlock.AddComponent<NumberBlock>(); }
            newBlock.GetComponent<Rigidbody>().velocity += launchVector;

            yield return new WaitForSeconds(waitTime / numberBlocks.Count);
        }
    }
    #endregion

    public IEnumerator Countdown()
    {
        //print("Starting Countdown");
        yield return new WaitForSeconds(waitTime);
        //print("Ended Countdown");
        NextRound();
    }

    public void CancelStart()
    {
        StopCoroutine(crDropBlocks);
        StopCoroutine(crCountdown);
        if (destroyBlocksOnCancel) { onCancelStart?.Invoke(); }
    }

    public void StartGame()
    {
        if (!starting)
        {
            //print("STARTING");
            crDropBlocks = DropBlocks();
            StartCoroutine(crDropBlocks);

            crCountdown = Countdown();
            StartCoroutine(crCountdown);

            //get all maps
            if (stages.stage1Sel) { selectedMaps.Add("Lab"); }
            if (stages.stage2Sel) { selectedMaps.Add("TrafficMap"); }
            if (stages.stage3Sel) { selectedMaps.Add("ConstructionMap"); }
            if (stages.stage4Sel) { selectedMaps.Add("VolcanoMap"); }
            if (stages.stage5Sel) { selectedMaps.Add("MallMap"); }
            if (stages.stage6Sel) { selectedMaps.Add("BalloonMap"); }
            if (selectedMaps == null) { selectedMaps.Add("Lab"); }
            selectedMaps = ShuffleList(selectedMaps);

            starting = true;
            return;
        }
        if (starting)
        {
            //print("CANCELLING");
            CancelStart();
            selectedMaps.Clear();

            starting = false;
            return;
        }
    }
    #endregion

    public void OnRoundEnd()
    {
        //update player scores etc
        NextRound();
    }

    public List<string> ShuffleList(List<string> toShuffle)
    {
        for (int i = 0; i < toShuffle.Count; i++)
        {
            string temp = toShuffle[i];
            int randIndex = Random.Range(i, toShuffle.Count);
            toShuffle[i] = toShuffle[randIndex];
            toShuffle[randIndex] = temp;
        }
        return toShuffle;
    }

    public void NextRound()
    {
        print("next round");
        if (currRound < numRounds)
        {
            print("LOAD SCENE: " + selectedMaps[currRound % selectedMaps.Count]);
            SceneManager.LoadScene(selectedMaps[currRound % selectedMaps.Count]);
        }
        if (currRound == numRounds)
        {
            print("LOAD SCENE: Victory");
            SceneManager.LoadScene("VictoryScreen");
        }

        currRound += 1;
        return;
    }

    public void Start()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            stats.Add(new playerStats());
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
