using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class initializePlayer : MonoBehaviour
{
    private PlayerConfiguration playerConfigs;
    public List<PartCombiner> partCombiners;

    //I need the finalized game object and the player number assigned to that
    // made a unity event in partcombiner


    // Start is called before the first frame update
    public void Initializer(int playerNum, GameObject player)
    {

        Debug.Log(player.name);
        var playerConfig = playerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        

        if (playerNum == 1)
        {        Debug.Log("PLAYERS CONFIGURE ACTION EVENT CALLED");

            //go into player controller on body, assign the player configuration to that body

             player.GetComponentInParent<PlayerController>().InitializePlayer(playerConfig[0]);
            
        }
        else if (playerNum == 2)
        {
            Debug.Log("PLAYER 2 CONFIGURATION");
            player.GetComponentInParent<PlayerController>().InitializePlayer(playerConfig[1]);
        }
        else if (playerNum == 3)
        {
            player.GetComponentInParent<PlayerController>().InitializePlayer(playerConfig[2]);
        }
        else if (playerNum == 4)
        {
            player.GetComponentInParent<PlayerController>().InitializePlayer(playerConfig[3]);
        } else { 

        Debug.Log(playerNum);

        }


        throw new NotImplementedException();
    }



    void Start()
    { 
        
        Debug.Log("Started this!!!");
        
        for (int i = 0; i < partCombiners.Count; i++) { 
            partCombiners[i].onCreatureSave.AddListener(Initializer);
            Debug.Log("Listener added");
           }
           
       

    }


}      
    
   
    




