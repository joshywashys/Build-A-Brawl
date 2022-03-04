using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetaStageSelect : MonoBehaviour
{
    public bool stage1Sel = false;
    public bool stage2Sel = false;
    public bool stage3Sel = false;
    public bool stage4Sel = false;

    public GameObject select;

    public static List<string> levels = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        select.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown(){
        if (this.name == "stage1" && stage1Sel == false){
            stage1Sel = true;
            levels.Add("Lab");
            select.SetActive(true);
            foreach( var x in levels) {
                Debug.Log( x.ToString());
            }
        } else if (this.name == "stage1" && stage1Sel == true){
            stage1Sel = false;
            levels.Remove("Lab");
            select.SetActive(false);
            foreach( var x in levels) {
                Debug.Log( x.ToString());
            }
        } 

        if (this.name == "stage2" && stage2Sel == false){
            stage2Sel = true;
            levels.Add("TrafficMap");
            select.SetActive(true);
            foreach( var x in levels) {
                Debug.Log( x.ToString());
            }
        } else if (this.name == "stage2" && stage2Sel == true){
            stage2Sel = false;
            levels.Remove("TrafficMap");
            select.SetActive(false);
            foreach( var x in levels) {
                Debug.Log( x.ToString());
            }
        }

        if (this.name == "stage3" && stage3Sel == false){
            stage3Sel = true;
            levels.Add("ConstructionMap");
            select.SetActive(true);
            foreach( var x in levels) {
                Debug.Log( x.ToString());
            }
        } else if (this.name == "stage3" && stage3Sel == true){
            stage3Sel = false;
            levels.Remove("ConstructionMap");
            select.SetActive(false);
            foreach( var x in levels) {
                Debug.Log( x.ToString());
            }
        }

        if (this.name == "stage4" && stage4Sel == false){
            stage4Sel = true;
            levels.Add("VolcanoMap");
            select.SetActive(true);
            foreach( var x in levels) {
                Debug.Log( x.ToString());
            }
        } else if (this.name == "stage4" && stage4Sel == true){
            stage4Sel = false;
            levels.Remove("VolcanoMap");
            select.SetActive(false);
            foreach( var x in levels) {
                Debug.Log( x.ToString());
            }
        }
    }
}
