using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//place on the text element you want to change the UI of
//temp debugging script
public class DisplayProfiles : MonoBehaviour
{
    private TextMeshProUGUI numProfilesText;
    public TMP_InputField nameInputField;
    public ProfileManager profileManager;

    private void Start()
    {
        numProfilesText = GetComponent<TextMeshProUGUI>();
        print(numProfilesText);
        print(numProfilesText.text);
        //print(profileManager.getNumProfiles());
    }

    void Update()
    {
        numProfilesText.text = profileManager.getNumProfiles().ToString();
    }
}
