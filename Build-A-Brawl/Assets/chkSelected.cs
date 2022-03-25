using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chkSelected : MonoBehaviour
{
    [SerializeField] public GameObject uiItem;
    // Start is called before the first frame update
    void Start()
    {
        uiItem.SetActive(false);
    }


    public void Selection ()
    {
        if (uiItem.activeSelf)
        {
            uiItem.SetActive(false);
        } else
        {
            uiItem.SetActive(true);
        }
    }

    // Update is called once per frame
    
}
