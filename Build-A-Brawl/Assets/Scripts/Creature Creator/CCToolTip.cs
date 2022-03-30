using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCToolTip : MonoBehaviour
{
    public PartCombiner creature;
    public GameObject symbolPrefab;
    public GameObject tooltip;
    private Camera cam;

    private GameObject selectedPart;
    private Vector3 offset = new Vector3(0, 20, 0);

    // Called on PartCombiner partswap
    public void UpdateSymbols()
    {
        // Clear debris from last update
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

    public void UpdatePosition()
    {
        Vector3 headHeight = new Vector3(0, creature.newHead.GetComponent<Collider>().bounds.size.y, 0);
        Vector3 targetPos = cam.WorldToScreenPoint(creature.newHead.transform.position + headHeight);
        tooltip.transform.position = targetPos + offset;
    }

    public void Start()
    {
        cam = FindObjectOfType<Camera>();

        creature.onPartSwap.AddListener(UpdateSymbols);

        Select();
    }

    public void Update()
    {
        UpdatePosition();
    }

    //maybe i can add in a function to make them appear with an animation

    public void Select()
    {
        tooltip.SetActive(true);
    }

    public void Deselect()
    {
        tooltip.SetActive(false);
    }
}
