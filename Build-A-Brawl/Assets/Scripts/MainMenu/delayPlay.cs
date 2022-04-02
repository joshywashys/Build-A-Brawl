using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delayPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("delayTheOpening", 5.0f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void delayTheOpening(){
        Destroy(this.gameObject);
    }
}
