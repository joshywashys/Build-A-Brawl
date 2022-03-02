using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CARRIES PLAYER CREATURES BETWEEN SCENES
public class CreatureManager : MonoBehaviour
{
    public const int MAX_PLAYERS = 4;
    [SerializeField] private GameObject[] creatures;
    //[SerializeField] private List<GameObject> creatures;

    public void AddCreature(GameObject toAdd, int playerNum)
    {
        RemoveCreature(playerNum);

        if (1 <= playerNum && playerNum <= MAX_PLAYERS)
        {
            GameObject newCreature = Instantiate(toAdd);
            newCreature.name = "Player " + (playerNum);
            creatures[playerNum - 1] = newCreature;
            DontDestroyOnLoad(newCreature);
            newCreature.SetActive(false);
        }
    }

    public void RemoveCreature(int playerNum)
    {
        if (1 <= playerNum && playerNum <= MAX_PLAYERS)
        {
            Destroy(creatures[playerNum - 1]);
            creatures[playerNum - 1] = null;
        }
    }

    #region Getters
    public GameObject GetCreature(int index)
    {
        return creatures[index];
    }

    public GameObject[] GetCreatureList()
    {
        return creatures;
    }
    #endregion

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        creatures = new GameObject[4];
    }

    //DEBUG
    public void debugSpawnCreature()
    {
        Instantiate(creatures[0]);
    }

}
