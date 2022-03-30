using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCToolTip : MonoBehaviour
{
    public PartCombiner creature;
    public GameObject tooltip;

    // Called on PartCombiner partswap
    public void UpdateSymbols()
    {
        //clear if there's any
        foreach (Transform child in tooltip.transform)
        {
            Destroy(child.gameObject);
        }

        // Generate new symbol list
        List<Image> symbols = creature.currHead.GetComponent<BodyPart>().partData.symbols;
        for (int i = 0; i < symbols.Count; i++)
        {
            Instantiate(symbols[i], tooltip.transform);
        }
    }

    public void Start()
    {
        // Get Tooltip!!!
        // Get Creature!!!

        creature.onPartSwap.AddListener(UpdateSymbols);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
