using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    public float waitTime;
    public List<GameObject> numberBlocks;
    public Transform blockSpawnpoint;

    public UnityEvent onCancelStart;

    public IEnumerator DropBlocks()
    {
        for (int i = 0; i < numberBlocks.Count; i++)
        {
            GameObject newBlock = Instantiate(numberBlocks[i], blockSpawnpoint);
            if (newBlock.GetComponent<NumberBlock>() == null) { newBlock.AddComponent<NumberBlock>(); }
            yield return new WaitForSeconds(waitTime / numberBlocks.Count);
        }
    }

    public void CancelStart()
    {
        StopCoroutine(DropBlocks());
        onCancelStart?.Invoke();
    }

    public void StartGame()
    {
        
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


}
