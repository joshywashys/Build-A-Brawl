using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugStartGame : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("CC Save Tester", LoadSceneMode.Single);
    }
}
