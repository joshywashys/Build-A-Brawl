using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class playerSetupMenuController : MonoBehaviour
{
    private int PlayerIndex;

    //text to change
    [SerializeField] private TextMeshProUGUI playerTitle;

    [SerializeField] private GameObject readyPanel;

    [SerializeField] private GameObject menuPanel;

    [SerializeField] private Button readyButton;

    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;

    //set player controller number, and change text
    public void SetPlayerIndex(int pi)
    {
        PlayerIndex = pi;
        playerTitle.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        //if not ignoring input, enable the input into the controllers
        if (Time.time > ignoreInputTime)
        {
           
            inputEnabled = true;
        }
    }

    public void finalized()
    {
        if(!inputEnabled)
        {
            Debug.Log("Input not enabled");
            return;
        }
        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(true);
        Debug.Log("Finalized");
    }

    public void readyPlayer()
    {
        if(!inputEnabled)
        {
            return;

        }

        playerConfigurationManager.Instance.readyPlayer(PlayerIndex);
        readyButton.gameObject.SetActive(false);
    }

}
