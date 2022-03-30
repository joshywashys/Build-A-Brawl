using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;

public class playerJoins : MonoBehaviour
{

    public GameObject playerJoinMenuPrefab;
    public PlayerInput input;

    // Start is called before the first frame update
    private void Awake()
    {
        var baseMenu = GameObject.Find("CharCreaMenu");
        if (baseMenu != null)
        {
            Debug.Log("IM TRTYING TO SPAWNN");
            var playerMenu = Instantiate(playerJoinMenuPrefab, baseMenu.transform);
            input.uiInputModule = playerMenu.GetComponentInChildren<InputSystemUIInputModule>();
            playerMenu.GetComponent<playerSetupMenuController>().SetPlayerIndex(input.playerIndex);
        }
    }
}
