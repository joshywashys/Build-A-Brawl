using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class playerJoins : MonoBehaviour
{

    public GameObject playerMenuType;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public List<PartCombiner> parts;

    public PlayerInput input;

    // Start is called before the first frame update


    void Awake()
    {
        playerJoining();
    }

    public void Start()
    {
        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].onFinalize.AddListener(SwitchInputs);
        }
    }

    public GameObject figure()
    {
        if (input.playerIndex == 0)
        {
            player1.SetActive(true);
            return player1;
        } else if (input.playerIndex == 1)
        {
            player2.SetActive(true);
            return player2;
        } else if (input.playerIndex == 2)
        {
            player3.SetActive(true);
            return player3;
        } else if (input.playerIndex == 3)
        {
            player4.SetActive(true);
            return player4;
        } else { }


        return null;
    }

    public void SwitchInputs(bool toggle, int playerNum)
    {
        if (toggle)
        {
            input.uiInputModule = parts[playerNum - 1].GetComponent<PlayerInput>();
            parts[playerNum - 1] = 
        } else if (!toggle)
        {

        }
    }

    public void playerJoining()
    {       
        
        var baseMenu = GameObject.Find("CharCreaMenu");
        if (baseMenu != null)
        {

            Debug.Log("IM TRTYING TO SPAWNN " + input.playerIndex);
            var playerMenu = figure();
            input.uiInputModule = playerMenu.GetComponentInChildren<InputSystemUIInputModule>();
            playerMenu.GetComponent<playerSetupMenuController>().SetPlayerIndex(input.playerIndex);
        }
    }

}