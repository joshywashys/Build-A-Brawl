using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCToolTip : MonoBehaviour
{
    public PartCombiner creature;
    public GameObject symbolPrefab;
    private GameObject tooltip;
    private Camera cam;

    private bool selected;
    private GameObject selectedPart;

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

    public void UpdatePosition(GameObject target)
    {
        Vector3 newPos = cam.WorldToScreenPoint(target.transform.position);
        tooltip.transform.position = newPos;
    }

    public void Start()
    {
        tooltip = gameObject;
        cam = FindObjectOfType<Camera>();

        creature.onPartSwap.AddListener(UpdateSymbols);

        Show(); // DEBUGGING
    }

    public void Update()
    {
        if (true)
        {
            selectedPart = creature.currHead; // DEBUGGING // replace with actual selected part
            UpdatePosition(selectedPart);
        }
    }

    //maybe i can add in a function to make them appear not all at once

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
