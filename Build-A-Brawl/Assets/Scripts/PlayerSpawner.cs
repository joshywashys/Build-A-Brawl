using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public RoundManager rm;
    [SerializeField] private CreatureManager creatureManager;
    [SerializeField] private List<Transform> spawnpoints;
    private PlayerConfiguration playerConfigs;

    void Awake()
    {
        creatureManager = FindObjectOfType<CreatureManager>();
        SpawnAllPlayers(creatureManager.GetCreatureList());
    }

    public void SpawnAllPlayers(GameObject[] toSpawn)
    {
        for (int i = 0; i < toSpawn.Length; i++)
        {
            if (toSpawn[i] != null)
            {

                SpawnPlayer(toSpawn[i], spawnpoints[i % spawnpoints.Count], i);
                
            }
        }
    }

    public void SpawnPlayer(GameObject toSpawn, Transform spawnpoint, int playerNum)
    {   

        var playerConfig = playerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

        GameObject newCreature = Instantiate(toSpawn, new Vector3(spawnpoint.position.x, spawnpoint.position.y, spawnpoint.position.z), Quaternion.identity);
        
        Debug.Log(toSpawn.name + " " + playerNum);
        Debug.Log(playerConfig[playerNum]);

        var player = newCreature;

        newCreature.SetActive(true);
        player.GetComponentInChildren<PlayerController>().InitializePlayer(playerConfig[playerNum]);


    }
}
