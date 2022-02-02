using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CARRIES PLAYER CREATURES BETWEEN SCENES
public class CreatureManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> creatures;

    public GameObject GetCreature(int index)
    {
        return creatures[index];
    }

    public List<GameObject> GetCreatureList()
    {
        return creatures;
    }

    public void AddCreature(GameObject toAdd)
    {
        if (creatures.Count < 4)
        {
            GameObject newCreature = Instantiate(toAdd);
            creatures.Add(newCreature);
            newCreature.SetActive(false);
            DontDestroyOnLoad(newCreature);
        }
        else { print("- MAX CREATURES - CREATURE NOT ADDED -"); }
        //instantiate
        //disable
    }

    public void ClearCreatures()
    {
        creatures.Clear();
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    //DEBUG
    public void debugSpawnCreature()
    {
        Instantiate(creatures[0]);
    }

}
