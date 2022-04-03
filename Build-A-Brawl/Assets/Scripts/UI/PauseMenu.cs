using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject PauseMenuObject;
    public AudioSource menuOpen;
    public bool allowPauseOpen = false;


    void Start(){
        InvokeRepeating("allowPause", 4, 1000);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (isPaused == true){
                Resume();
            } else {
                Pause();
            }
        }
    }

    void allowPause(){
        allowPauseOpen = true;
    }

    public void Resume(){
        if (allowPauseOpen == true)
        {
            PauseMenuObject.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            menuOpen.Play();
        }
        
    }
    void Pause(){
        if (allowPauseOpen == true)
        {
            PauseMenuObject.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            menuOpen.Play();
        }
    }
    public void ReturntoMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("MM for Jacob");
    }
}
