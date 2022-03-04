using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugObjSpawner : MonoBehaviour
{
    public GameObject toSpawn;
    public bool stopSpawning = false;
    public float spawnTime = 5;
    public float spawnDelay = 5;

    void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
    }

    public void SpawnObject()
    {
        Instantiate(toSpawn, transform.position, transform.rotation);
        if (stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }
    }

}
