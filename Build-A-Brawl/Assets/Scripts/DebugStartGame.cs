using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugStartGame : MonoBehaviour
{
    public static int betaDemoSceneNum = 1;

    [SerializeField] private CreatureManager cm;

    void Start()
    {
        cm = FindObjectOfType<CreatureManager>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("ConstructionMap", LoadSceneMode.Single);
    }

    public void BetaDemoLoadScene()
    {
        switch(betaDemoSceneNum)
        {
            case 1:
                SceneManager.LoadScene("Lab", LoadSceneMode.Single);
                betaDemoSceneNum++;
                break;
            case 2:
                cm.RemoveCreature(2);
                cm.RemoveCreature(3);
                cm.RemoveCreature(4);
                SceneManager.LoadScene("ConstructionMap", LoadSceneMode.Single);
                betaDemoSceneNum++;
                break;
            case 3:
                SceneManager.LoadScene("VolcanoMap", LoadSceneMode.Single);
                betaDemoSceneNum++;
                break;
            case 4:
                SceneManager.LoadScene("TrafficMap", LoadSceneMode.Single);
                betaDemoSceneNum++;
                break;
            case 5:
                SceneManager.LoadScene("Lab", LoadSceneMode.Single);
                betaDemoSceneNum++;
                break;
        }
    }
}
