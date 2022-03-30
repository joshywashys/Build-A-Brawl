using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCToolTip : MonoBehaviour
{
    public PartCombiner creature;
    public GameObject symbolPrefab;
    private GameObject tooltip;

    // Called on PartCombiner partswap
    public void UpdateSymbols()
    {
        //clear if there's any
        foreach (Transform child in tooltip.transform)
        {
            Destroy(child.gameObject);
        }

        // Generate new symbol list
        List<Sprite> symbols = creature.currHead.GetComponent<BodyPart>().partData.symbols;
        print(symbols);
        for (int i = 0; i < symbols.Count; i++)
        {
            print("added symbol " + i);
            Instantiate(symbolPrefab, tooltip.transform);
            symbolPrefab.GetComponent<Image>().sprite = symbols[i];
        }
    }

    public void Start()
    {
        tooltip = gameObject;

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
