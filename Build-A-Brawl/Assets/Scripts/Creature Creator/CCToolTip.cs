using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCToolTip : MonoBehaviour
{
    [Header("Drag shit in here lmao")]
    public PartCombiner creature;
    public GameObject symbolPrefab;
    public GameObject currSymbol;
    public GameObject tooltip;
    
    private Camera cam;

    private GameObject selectedPart;
    private bool selected = true;
    private Vector3 offset = new Vector3(0, 20, 0);

    /*
    [Header("Color Settings")]
    public Color startColor;
    public Color endColor;
    public float lerpPeriod;
    */

    // Called on PartCombiner partswap
    public void UpdateSymbols()
    {
        // Clear debris from last update
        foreach (Transform child in tooltip.transform)
        {
            Destroy(child.gameObject);
        }

        // Generate new symbol list
        List<Sprite> symbols = creature.newHead.GetComponent<BodyPart>().partData.symbols;
        for (int i = 0; i < symbols.Count; i++)
        {
            print("added symbol " + symbols[i].name);
            currSymbol = Instantiate(symbolPrefab, tooltip.transform);
            currSymbol.GetComponent<Image>().sprite = symbols[i];
        }
    }

    public void UpdatePosition()
    {
        Vector3 headHeight = new Vector3(0, creature.newHead.GetComponent<Collider>().bounds.size.y, 0);
        Vector3 targetPos = cam.WorldToScreenPoint(creature.newHead.transform.position + headHeight);
        tooltip.transform.position = targetPos + offset;
    }

    public void UpdateColor()
    {
        //tooltip.GetComponent<Image>().color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time / lerpPeriod, 1));
    }

    public void Start()
    {
        //Select();

        currSymbol = symbolPrefab;
        cam = FindObjectOfType<Camera>();

        creature.onPartSwap.AddListener(UpdateSymbols);
        creature.onFinalize.AddListener(Toggle);
    }

    public void Update()
    {
        if (selected) { UpdatePosition(); }
        //UpdateColor(); was just trying for fun hehe
    }

    //maybe i can add in a function to make them appear with an animation

    public void Toggle(bool toggleType)
    {
        switch(toggleType)
        {
            case true: Deselect(); break;
            case false: Select(); break;
        }
    }

    public void Select()
    {
        selected = true;
        tooltip.SetActive(true);
    }

    public void Deselect()
    {
        //tooltip.GetComponent<Image>().color = startColor;

        selected = false;
        tooltip.SetActive(false);
    }
}
