using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CARRIES PLAYER CREATURES BETWEEN SCENES
public class CreatureManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> creatures;

    public void AddCreature(GameObject toAdd)
    {
        if (creatures.Count < 4)
        {
            GameObject newCreature = Instantiate(toAdd);
            newCreature.name = "Player " + (creatures.Count + 1);
            creatures.Add(newCreature);
            newCreature.SetActive(false);
            DontDestroyOnLoad(newCreature);
        }
        else { print("- MAX CREATURES - CREATURE NOT ADDED -"); }
    }

    public void ClearCreatures()
    {
        creatures.Clear();
    }

    #region Getters
    public GameObject GetCreature(int index)
    {
        return creatures[index];
    }

    public List<GameObject> GetCreatureList()
    {
        return creatures;
    }
    #endregion

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
