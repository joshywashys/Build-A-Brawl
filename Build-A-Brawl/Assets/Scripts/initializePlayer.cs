using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initializePlayer : MonoBehaviour
{
    [SerializeField] public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void playerOne ()
    {
            var playerConfigs = playerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
            player.GetComponent<PlayerController>().InitializePlayer(playerConfigs[0]);
        
    }
}
