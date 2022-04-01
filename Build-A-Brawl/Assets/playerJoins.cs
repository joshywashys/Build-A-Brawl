using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;

public class playerJoins : MonoBehaviour
{

    public GameObject playerMenuType;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;


    public PlayerInput input;

    // Start is called before the first frame update


    void Awake()
    {
        playerJoining();
    }

    public GameObject figure()
    {
        if (input.playerIndex == 0)
        {

            return player1;
        } else if (input.playerIndex == 1)
        {

            return player2;
        } else if (input.playerIndex == 2)
        {

            return player3;
        } else if (input.playerIndex == 3)
        {

            return player4;
        } else { }


        return null;
    }

    public void playerJoining()
    {       
        
        var baseMenu = GameObject.Find("CharCreaMenu");
        if (baseMenu != null)
        {
        
            Debug.Log("IM TRTYING TO SPAWNN");
            var playerMenu = Instantiate(figure(), baseMenu.transform);
            input.uiInputModule = playerMenu.GetComponentInChildren<InputSystemUIInputModule>();
            playerMenu.GetComponent<playerSetupMenuController>().SetPlayerIndex(input.playerIndex);
        }
    }

}