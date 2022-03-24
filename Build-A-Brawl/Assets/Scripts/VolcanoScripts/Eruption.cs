using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eruption : MonoBehaviour
{
   public GameObject meteor, spawnLocation, eruption, eruptionLocation;

    public float repeat;
    public float delay;
    //public int[] prevLoc;
    public List<int> prevLoc = new List<int>();
    int randomNum;

    void Start()
    {
        InvokeRepeating("spawnEnemy", delay, repeat);
    }

    public void spawnEnemy(){
        GameObject _eruption = Instantiate(eruption, eruptionLocation.transform.position, eruptionLocation.transform.rotation);
        Destroy(_eruption, 10);
        checkSpawn();
        Instantiate(meteor, spawnLocation.transform.position, spawnLocation.transform.rotation);
        checkSpawn();
        Instantiate(meteor, spawnLocation.transform.position, spawnLocation.transform.rotation);
        checkSpawn();
        Instantiate(meteor, spawnLocation.transform.position, spawnLocation.transform.rotation);
        checkSpawn();
        Instantiate(meteor, spawnLocation.transform.position, spawnLocation.transform.rotation);
        checkSpawn();
        Instantiate(meteor, spawnLocation.transform.position, spawnLocation.transform.rotation);
        prevLoc.Clear();
    }

    public void checkSpawn(){
                randomNum = Random.Range(0, 32);
        for (int i = 0; i < prevLoc.Count;){
            if (randomNum == prevLoc[i]){
                randomNum = Random.Range(0, 32);
                i = 0;
            }
            else {
                i++;
            }
        }
        prevLoc.Add(randomNum);
        spawnLocation = GameObject.Find("grid (" + randomNum + ")");
    }
}
