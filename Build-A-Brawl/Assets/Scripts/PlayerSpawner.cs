using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private CreatureManager creatureManager;
    [SerializeField] private List<Transform> spawnpoints;

    // Start is called before the first frame update
    void Start()
    {
        creatureManager = FindObjectOfType<CreatureManager>();
        if (creatureManager != null)
        {
            print("success!!");
        }

        //DEBUGGING
        SpawnAllPlayers(creatureManager.GetCreatureList());
    }

    public void SpawnAllPlayers(List<GameObject> toSpawn)
    {
        for (int i = 0; i < toSpawn.Count; i++)
        {
            SpawnPlayer(toSpawn[i], spawnpoints[i % spawnpoints.Count]);
        }
    }

    public void SpawnPlayer(GameObject toSpawn, Transform spawnpoint)
    {
        GameObject newCreature = Instantiate(toSpawn, new Vector3(spawnpoint.position.x, spawnpoint.position.y, spawnpoint.position.z), Quaternion.identity);
        newCreature.SetActive(true);
    }
}
