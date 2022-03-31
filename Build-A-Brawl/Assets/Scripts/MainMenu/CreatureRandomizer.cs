using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureRandomizer : MonoBehaviour
{
    public PartCombiner pc;
    public float switchPeriod = 1f;
    public float switchRate = 4;

    IEnumerator ScrambleTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(switchRate);
            StartCoroutine(ScrambleCreature());
        }
    }

    IEnumerator ScrambleCreature()
    {
        #if true
        int headScrambleNum = Random.Range(1, PartCombiner.partsList[BundleNameCache.creaturepartsHeads].Length);
        int torsoScrambleNum = Random.Range(1, PartCombiner.partsList[BundleNameCache.creaturepartsTorsos].Length);
        int armLScrambleNum = Random.Range(1, PartCombiner.partsList[BundleNameCache.creaturepartsArmsL].Length);
        int armRScrambleNum = Random.Range(1, PartCombiner.partsList[BundleNameCache.creaturepartsArmsR].Length);
        int legsScrambleNum = Random.Range(1, PartCombiner.partsList[BundleNameCache.creaturepartsLegs].Length);
        #else
        int headScrambleNum = Random.Range(1, 3);
        int torsoScrambleNum = Random.Range(1, 3);
        int armLScrambleNum = Random.Range(1, 3);
        int armRScrambleNum = Random.Range(1, 3);
        int legsScrambleNum = Random.Range(1, 3);
        #endif
        int totalScrambleCount = headScrambleNum + torsoScrambleNum + armLScrambleNum + armRScrambleNum + legsScrambleNum;
        float switchSpeed = switchPeriod / totalScrambleCount;

        for (int i = 0; i < headScrambleNum; i++)
        {
            pc.NextHead();
            yield return new WaitForSeconds(switchSpeed);
        }
        
        for (int i = 0; i < torsoScrambleNum; i++)
        {
            pc.NextTorso();
            yield return new WaitForSeconds(switchSpeed);
        }
        
        for (int i = 0; i < armLScrambleNum; i++)
        {
            pc.NextArmL();
            yield return new WaitForSeconds(switchSpeed);
        }
        
        for (int i = 0; i < armRScrambleNum; i++)
        {
            pc.NextArmR();
            yield return new WaitForSeconds(switchSpeed);
        }
        
        for (int i = 0; i < legsScrambleNum; i++)
        {
            pc.NextLegs();
            yield return new WaitForSeconds(switchSpeed);
        }
    }

    void Start()
    {
        StartCoroutine(ScrambleTimer());
    }

    private void OnEnable()
    {
        StartCoroutine(ScrambleTimer());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

}
