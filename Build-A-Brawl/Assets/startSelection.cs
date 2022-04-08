using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class startSelection : MonoBehaviour
{
    public Button button;
    [SerializeField] public bool inMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(buttonSelect());
        inMenu = true;
    }

  IEnumerator buttonSelect()
    {
        yield return 0;
        button.Select();
        Debug.Log("Selected");
    }

}
