using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public BetaStageSelect stages;
    public List<string> selectedMaps;
    public List<string> randomMapPool;
    public List<int> selectedMapIndexes;
    public int numRounds = 5;
    private int currRound = 0;

    public class playerStats
    {
        public decimal score;
        public bool isAlive;
    }

    public int numPlayers;
    public List<playerStats> stats;
    public List<CreatureStats> players;

    #region Main Menu
    [Header("Main Menu")]
    public float waitTime;
    public float onRoundEndWait;

    public UnityEvent onCancelStart;

    private bool starting = false;
    private IEnumerator crDropBlocks;
    private IEnumerator crCountdown;
    private IEnumerator crRoundEnd;

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

            selectedMaps = new List<string>();
            selectedMaps = BetaStageSelect.levels;

            /*
            foreach (string str in selectedMaps)
            {
                print("level: " + str);
            }
            */

            //get all maps
            /*
            if (stages.stage1Sel) { selectedMaps.Add("Lab"); }
            if (stages.stage2Sel) { selectedMaps.Add("TrafficMap"); }
            if (stages.stage3Sel) { selectedMaps.Add("ConstructionMap"); }
            if (stages.stage4Sel) { selectedMaps.Add("VolcanoMap"); }
            if (stages.stage5Sel) { selectedMaps.Add("MallMap"); }
            if (stages.stage6Sel) { selectedMaps.Add("BalloonMap"); }
            if (selectedMaps == null) { selectedMaps.Add("Lab"); }

            print(stages.stage1Sel);
            print(stages.stage2Sel);
            print(stages.stage3Sel);
            print(stages.stage4Sel);
            print(stages.stage5Sel);
            print(stages.stage6Sel);
            */

            //selectedMaps.Add("Lab");
            //selectedMaps.Add("TrafficMap");
            //selectedMaps.Add("ConstructionMap");
            //selectedMaps.Add("VolcanoMap");
            //selectedMaps.Add("MallMap");
            //selectedMaps.Add("BalloonMap");

            //print(selectedMaps.Count);
            /*
            for (int i = 0; i < selectedMaps.Count; i++)
            {
                print(selectedMaps[i]);
            }
            */

            /*
            selectedMapIndexes = new List<int>();
            for (int i = 0; i < numRounds; i++)
            {
                selectedMapIndexes.Add(Random.Range(0,selectedMaps.Count));
            }
            */

            // Randomize Maps
            randomMapPool = new List<string>();
            randomMapPool = selectedMaps;
            if (randomMapPool.Count > 0)
            {
                for (int i = 0; i < randomMapPool.Count; i++)
                {
                    int randNum = Random.Range(0, randomMapPool.Count);
                    string temp = randomMapPool[i];
                    randomMapPool[i] = randomMapPool[randNum];
                    randomMapPool[randNum] = randomMapPool[i];
                }
            }
            if (randomMapPool.Count == 0)
            {
                randomMapPool.Add("Lab");
            }
            //print("(StartGame) round num: " + currRound);
            //print("(StartGame) pool count: " + randomMapPool.Count);

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

    public void UpdateScore(int playerNum)
    {
        print("updating scores");
        print(stats.Count);
        stats[playerNum - 1].isAlive = false;

        // Check if one person standing
        int numDead = 0;
        for (int i = 0; i < stats.Count; i++)
        {
            print("in tha loop");
            if (stats[i].isAlive == false)
            {
                print("someone died lmao :(");
                numDead += 1;
            }
            if (numDead == stats.Count - 1)
            {
                print("round ending");
                OnRoundEnd();
            }
        }
    }

    public IEnumerator RoundEndTimer()
    {
        print("waiting");
        yield return new WaitForSeconds(onRoundEndWait);
        print("done waiting");
        NextRound();
    }

    public void OnRoundEnd()
    {
        //update player scores etc
        for (int i = 0; i < stats.Count; i++)
        {
            if (stats[i].isAlive == true)
            {
                stats[i].score += 1;
            }
        }
        for (int i = 0; i < stats.Count; i++)
        {
            if (players[i] != null) { players[i].onDeath.RemoveListener(UpdateScore); }
        }

        print("round ending!!!");
        crRoundEnd = RoundEndTimer();
        StartCoroutine(RoundEndTimer());
    }

    public void NextRound()
    {
        if (currRound < numRounds)
        {
            //print("selectedMaps.Count: " + selectedMaps.Count);
            //print("currRound: " + currRound);
            //print("LOAD SCENE: " + selectedMaps[currRound % selectedMaps.Count]); // % selectedMaps.Count


            //SceneManager.LoadScene(selectedMaps[selectedMapIndexes[numRounds % selectedMapIndexes.Count] % selectedMaps.Count]);
            // seelcted maps, selected map indexes (rand list), currRound
            //print("(NextRound) round num: " + currRound);
            print("(NextRound) pool count: " + randomMapPool.Count);
            SceneManager.LoadScene(randomMapPool[currRound % randomMapPool.Count]);
        }
        if (currRound == numRounds)
        {
            print("LOAD SCENE: Victory");
            SceneManager.LoadScene("VictoryScreen");
        }
        currRound += 1;
    }

    public void Start()
    {
        CreatureStats[] searchList = FindObjectsOfType<CreatureStats>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < searchList.Length; j++)
            {
                if (searchList[j].GetPlayerNum() == i + 1)
                {
                    players.Add(searchList[j]);
                    players[i].onDeath.AddListener(UpdateScore);
                }
            }
        }

        stats = new List<playerStats>();

        for (int i = 0; i < numPlayers; i++)
        {
            stats.Add(new playerStats());
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();

        players.Clear();
        CreatureStats[] searchList = FindObjectsOfType<CreatureStats>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < searchList.Length; j++)
            {
                if (searchList[j].GetPlayerNum() == i + 1)
                {
                    players.Add(searchList[j]);
                    players[i].onDeath.AddListener(UpdateScore);
                }
            }
        }
        //print("player " + players[0].playerNum + " detected");

        for (int i = 0; i < stats.Count; i++)
        {
            stats[i].isAlive = true;
            print(i);
        }

        //Debug.Log("OnSceneLoaded: " + scene.name);
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StopAllCoroutines();
    }

}
