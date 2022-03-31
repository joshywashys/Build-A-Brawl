using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    [Header("Main Menu")]
    public float waitTime;

    public UnityEvent onCancelStart;

    #region Menus
    [Header("Main Menu Blocks")]
    public List<GameObject> numberBlocks;
    public Transform blockSpawnpoint;
    public bool useRandomRotation;
    public Vector3 launchVector;
    private bool starting = false;

    public IEnumerator DropBlocks()
    {
        for (int i = 0; i < numberBlocks.Count; i++)
        {
            Quaternion qt = Quaternion.identity;
            if (useRandomRotation) { qt = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)); }

            GameObject newBlock = Instantiate(numberBlocks[i], blockSpawnpoint.transform.position, qt, blockSpawnpoint);
            if (newBlock.GetComponent<NumberBlock>() == null) { newBlock.AddComponent<NumberBlock>(); }
            newBlock.GetComponent<Rigidbody>().velocity += launchVector;

            yield return new WaitForSeconds(waitTime / numberBlocks.Count);
        }
    }

    public IEnumerator Countdown()
    {
        yield return new WaitForSeconds(waitTime);

    }

    public void CancelStart()
    {
        StopCoroutine(DropBlocks());
        StopCoroutine(Countdown());
        onCancelStart?.Invoke();
    }

    public void StartGame()
    {
        if (!starting)
        {
            StartCoroutine(DropBlocks());
            StartCoroutine(Countdown());
        }
        if (starting)
        {
            CancelStart();
        }
    }
    #endregion

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


}
