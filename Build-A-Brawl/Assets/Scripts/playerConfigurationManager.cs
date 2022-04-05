/* this code follows a tutorial at this link: https://www.youtube.com/watch?v=_5pOiYHJgl0&t=387s */
/* Anna S */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerConfigurationManager : MonoBehaviour
{
    //this is the list of player configurations as a whole (ie the whole dataset of player 1, the whole dataset of player 2, etc.)
    private List<PlayerConfiguration> playerConfigs;

    //sets the max players in the scene
    [SerializeField] private int MaxPlayers = 4;


    //sets an instance of the class to get the entirety of the data.
    public static playerConfigurationManager Instance { get; private set; }

  

    private void Awake()
    {

        if(Instance != null)
        {
            Debug.Log("Trying to create another instance of bad bad things");
        } else
        {
            Instance = this;
            //stops instance from being destroyed acrsos scenes, i.e. keeps player info
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    public void readyPlayer(int index)
    {
        //if ready button clicked, ready player is called
        playerConfigs[index].isReady = true;

        //if all players are ready,there is a countdown
        if (playerConfigs.All(p => p.isReady == true))
        {
            Debug.Log("READT TO BRAWL BRO");
        }
    }

    //get the list of player configurations
    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    //this is where player index is assigned
    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player " + pi.playerIndex + " joined!");

        
            //checking if player added yet
            if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
            {
                //puts the player input into the transform (script-attached object)
                //  pi.transform.parent.SetParent(transform);
                playerConfigs.Add(new PlayerConfiguration(pi));



           
        }
    }

}

public class PlayerConfiguration { 
    //this attaches the player input (controller) to the player index
    //we can now call the control with input and the player with playerindex
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }

    //playerInput takes in controllers, we are storing that action into Input
    public PlayerInput Input { get; set; }

    //this is where we store the playerindex, what number the player is
    public int PlayerIndex { get; set; }

    //this checks if a player is ready or not
    public bool isReady { get; set; }

}
