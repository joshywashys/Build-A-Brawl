/* this code follows a tutorial at this link: https://www.youtube.com/watch?v=_5pOiYHJgl0&t=387s */
/* Anna S */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

    [SerializeField] private int MaxPlayers = 4;

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
        playerConfigs[index].isReady = true;

        //if all players are ready,there is a countdown
        if (playerConfigs.All(p => p.isReady == true))
        {
            Debug.Log("READT TO BRAWL BRO");
        }
    }

    //this is where player index is assigned
    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player " + pi.playerIndex + " joined!");


        //checking if player added yet
        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {     
            
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi));
        }
    }


}

public class PlayerConfiguration { 
    
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool isReady { get; set; }

}
