using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chkSelected : MonoBehaviour
{

    [SerializeField] GameObject uiItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Selection ()
    {
        if (!uiItem.activeSelf)
        {
            uiItem.SetActive(true);
        } else
        {
            uiItem.SetActive(false);
        }
    }
}
