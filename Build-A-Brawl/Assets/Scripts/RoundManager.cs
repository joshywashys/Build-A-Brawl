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
    public bool destroyBlocksOnCancel;
    public Vector3 launchVector;

    private bool starting = false;
    private IEnumerator crDropBlocks;
    private IEnumerator crCountdown;

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
        StopCoroutine(crDropBlocks);
        StopCoroutine(crCountdown);
        if (destroyBlocksOnCancel) { onCancelStart?.Invoke(); }
    }

    public void StartGame()
    {
        if (!starting)
        {
            print("STARTING");
            crDropBlocks = DropBlocks();
            StartCoroutine(crDropBlocks);

            crCountdown = Countdown();
            StartCoroutine(crCountdown);

            starting = true;
            return;
        }
        if (starting)
        {
            print("CANCELLING");
            CancelStart();
            starting = false;
            return;
        }
    }
    #endregion

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


}
