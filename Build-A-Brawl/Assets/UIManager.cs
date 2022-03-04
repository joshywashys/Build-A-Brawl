using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//deal with UI ingame
public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    private List<CreatureStats> statsList;

    [Header("Health Overlay")]
    [SerializeField] private GameObject overlayPrefab;
    private List<GameObject> overlays;
    [SerializeField] private List<Image[]> limbImgs;
    public List<Color> hitColors;
    public List<Color> playerColors;
    
    public void CalculateHitColour(int playerNum, int limbNum, int health, int healthMax)
    {
        for (int i = 0; i < hitColors.Count; i++)
        {
            if (health < (healthMax / hitColors.Count) * i)
            {
                //limbImgs[playerNum].color = hitColors[i];
            }
        }
    }

    public void UpdateUI()
    {

    }

    void Start()
    {
        // Initialize
        statsList = new List<CreatureStats>();
        overlays = new List<GameObject>();
        limbImgs = new List<Image[]>();

        // Get players stats
        CreatureStats[] searchList = FindObjectsOfType<CreatureStats>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < searchList.Length; j++)
            {
                if (searchList[j].GetPlayerNum() == i + 1)
                {
                    statsList.Add(searchList[j]);
                }
            }
        }

        // Instantiate canvas elements
        for (int i = 0; i < statsList.Count; i++)
        {
            GameObject currOverlay = Instantiate(overlayPrefab, canvas.transform);
            //overlays[i].transform.position = new Vector3(0, (Screen.width / statsList.Count) * i, 0);
            currOverlay.GetComponent<RectTransform>().localPosition = new Vector3(0, (Screen.height/2) * 0.8f, 0);
            overlays.Add(currOverlay);
        }

        // Initialize Arrays in img list
        for (int i = 0; i < overlays.Count; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                limbImgs.Add(new Image[5]);
            }
        }

        // Set Player Colours
        for (int i = 0; i < overlays.Count; i++)
        {
            overlays[i].transform.Find("bg").GetComponent<Image>().color = playerColors[Random.Range(0,5)];
        }

        // Get limb imgs
        for (int i = 0; i < overlays.Count; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                limbImgs[i][j] = overlays[i].transform.Find("limbs").GetChild(j).GetComponent<Image>();
            }
        }

    }

    void Update()
    {
        
    }

}
