using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class initializePlayer : MonoBehaviour
{
    private PlayerConfiguration playerConfigs;
    public List<PartCombiner> partCombiners;
    
        

[SerializeField] public GameObject player;
  //  PlayerController pc = body.GetComponent<PlayerController>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < partCombiners.Count; i++)
        {
            //partCombiners[i].onSaveCreature.AddListener(Initializer());
        }

    }

    public void Initializer()
    {
        var playerConfig = playerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

        if (playerConfigs.PlayerIndex == 0)
        {
            Debug.Log("Configured to the first player");
            //go into player controller on body, assign the player configuration to that body
          //  pc.GetComponent<PlayerController>().InitializePlayer(playerConfig[0]);

        }
        else if (playerConfigs.PlayerIndex == 1)
        {
          //  pc.GetComponent<PlayerController>().InitializePlayer(playerConfig[1]);

        }
        else if (playerConfigs.PlayerIndex == 2)
        {
          //  pc.GetComponent<PlayerController>().InitializePlayer(playerConfig[2]);

        }
        else if (playerConfigs.PlayerIndex == 3)
        {
           // pc.GetComponent<PlayerController>().InitializePlayer(playerConfig[3]);
        }
    }
}
