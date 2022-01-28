using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> creatures;

    public GameObject GetCreature(int index)
    {
        return creatures[index];
    }

    public void AddCreature(GameObject toAdd)
    {
        if (creatures.Count < 4)
        {
            creatures.Add(toAdd);
        }
        else { print("- MAX CREATURES - CREATURE NOT ADDED -"); }
    }

    public void ClearCreatures()
    {
        creatures.Clear();
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
