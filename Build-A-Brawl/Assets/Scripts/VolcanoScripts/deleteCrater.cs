using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteCrater : MonoBehaviour
{
    // Start is called before the first frame update
    private float curScale;
    public float startScale;
    public float shrinkSpeed;
    public GameObject thisCrater;

    void Start()
    {
        StartCoroutine(destroyMe());
    }

    IEnumerator destroyMe()
    {
        yield return new WaitForSeconds(10.0f);
        Destroy(thisCrater);
    }
    // Update is called once per frame
    void Update()
    {
        curScale = Mathf.MoveTowards(curScale, startScale, Time.deltaTime * shrinkSpeed);
        gameObject.transform.localScale = new Vector3(curScale, curScale, curScale);
    }
}
