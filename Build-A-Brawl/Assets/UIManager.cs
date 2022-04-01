using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
    private Text playerNumText;
    
    public Color CalculateHitColour(float health, float healthMax)
    {
        Color toReturn = Color.green; //default for debugging
        for (int i = 0; i < hitColors.Count; i++)
        {
            if (health >= (healthMax / hitColors.Count) * i)
            {
                toReturn = hitColors[hitColors.Count - 1 - i];
            }
            else if (health < 0)
            {
                print("dead colours");
                toReturn = hitColors[hitColors.Count - 1];
                return toReturn;
            }
        }

        return toReturn;
    }

    public void UpdateUI(int playerNum)
    {
        limbImgs[playerNum - 1][0].color = CalculateHitColour(statsList[playerNum - 1].GetHealthHead(), statsList[playerNum - 1].GetHealthHeadMax());
        limbImgs[playerNum - 1][1].color = CalculateHitColour(statsList[playerNum - 1].GetHealthTorso(), statsList[playerNum - 1].GetHealthTorsoMax());
        limbImgs[playerNum - 1][2].color = CalculateHitColour(statsList[playerNum - 1].GetHealthArmL(), statsList[playerNum - 1].GetHealthArmLMax());
        limbImgs[playerNum - 1][3].color = CalculateHitColour(statsList[playerNum - 1].GetHealthArmR(), statsList[playerNum - 1].GetHealthArmRMax());
        limbImgs[playerNum - 1][4].color = CalculateHitColour(statsList[playerNum - 1].GetHealthLegs(), statsList[playerNum - 1].GetHealthLegsMax());
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
            currOverlay.GetComponent<RectTransform>().localPosition = new Vector3((Screen.width / (statsList.Count + 1)) * (i + 1) - Screen.width / 2, -(Screen.height/2) * 0.8f, 0);
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
            overlays[i].transform.Find("bg").GetComponent<Image>().color = playerColors[i];
        }

        for (int i = 0; i < overlays.Count; i++)
        {
            overlays[i].transform.Find("playerNum").GetComponent<Text>().text = "Player " + (i + 1);
        }

        // Get limb imgs
        for (int i = 0; i < overlays.Count; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                limbImgs[i][j] = overlays[i].transform.Find("limbs").GetChild(j).GetComponent<Image>();
            }
        }

        // Subscribe to all CreatureStats events
        for (int i = 0; i < statsList.Count; i++)
        {
            statsList[i].onDamage.AddListener(UpdateUI);
        }

    }

    void Update()
    {
        
    }

}
