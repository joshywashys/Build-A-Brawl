using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureRandomizer : MonoBehaviour
{
    public PartCombiner pc;
    public float period;
    public float switchRate;

    public void ScrambleCreature()
    {
        int headScrambleNum = Random.Range(1, PartCombiner.partsList[BundleNameCache.creaturepartsHeads].Length);
        for (int i = 0; i < headScrambleNum; i++)
        {
            pc.NextHead();
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
