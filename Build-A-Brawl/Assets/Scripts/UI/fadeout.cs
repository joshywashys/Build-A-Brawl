using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fadeout : MonoBehaviour
{

    public SpriteRenderer blackScreen;
    public float fade = 0;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("fadetoBlack", 25.0f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
       if (fade > 1){
          SceneManager.LoadScene("Main Menu", LoadSceneMode.Additive);
       }
    }

    void fadetoBlack()
        {
            Color tmp = blackScreen.GetComponent<SpriteRenderer>().color;
            tmp.a = fade;
            blackScreen.GetComponent<SpriteRenderer>().color = tmp;
            fade = fade + 0.01f;
            Invoke("fadetoBlack", 0.01f);
        }
}
