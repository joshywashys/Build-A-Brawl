using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetaStageSelect : MonoBehaviour
{
    public bool stage1Sel = false;
    public bool stage2Sel = false;
    public bool stage3Sel = false;
    public bool stage4Sel = false;
    public bool stage5Sel = false;
    public bool stage6Sel = false;

    public Material Material1;
    public Material Material2;
    public Material Material3;
    public Material Material4;
    public Material Material5;
    public Material Material6;
    public Material nothingSelected;
    //public GameObject thumbNail;
    public Image thumbNail;

    public GameObject select;

    public static List<string> levels = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        //thumbNail = GameObject.Find("thumbNail");
    }

    // Update is called once per frame
    void Update()
    {
        if (levels.Count == 0){
            thumbNail.GetComponent<Image>().material = nothingSelected;
        }
    }
    void OnMouseDown(){
        if (this.name == "stage1" && stage1Sel == false){
            stage1Sel = true;
            levels.Add("Lab");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material1;
            }
            //thumbNail.GetComponent<Image>().material = Material1;
        } else if (this.name == "stage1" && stage1Sel == true){
            stage1Sel = false;
            levels.Remove("Lab");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material1;
            }
        } 

        if (this.name == "stage2" && stage2Sel == false){
            stage2Sel = true;
            levels.Add("TrafficMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material2;
            }
        } else if (this.name == "stage2" && stage2Sel == true){
            stage2Sel = false;
            levels.Remove("TrafficMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material2;
            }
        }

        if (this.name == "stage3" && stage3Sel == false){
            stage3Sel = true;
            levels.Add("ConstructionMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material3;
            }
        } else if (this.name == "stage3" && stage3Sel == true){
            stage3Sel = false;
            levels.Remove("ConstructionMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material3;
            }
        }

        if (this.name == "stage4" && stage4Sel == false){
            stage4Sel = true;
            levels.Add("VolcanoMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material4;
            }
        } else if (this.name == "stage4" && stage4Sel == true){
            stage4Sel = false;
            levels.Remove("VolcanoMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material4;
            }
        }

        if (this.name == "stage5" && stage5Sel == false){
            stage5Sel = true;
            levels.Add("MallMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material5;
            }
        } else if (this.name == "stage5" && stage5Sel == true){
            stage5Sel = false;
            levels.Remove("MallMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material5;
            }
        }

        if (this.name == "stage6" && stage6Sel == false){
            stage6Sel = true;
            levels.Add("BalloonMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material6;
            }
        } else if (this.name == "stage6" && stage6Sel == true){
            stage6Sel = false;
            levels.Remove("BalloonMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material6;
            }
        }
    }

    public void levelSelect(){
        if (this.name == "stage1" && stage1Sel == false){
            stage1Sel = true;
            levels.Add("Lab");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material1;
            }
        } else if (this.name == "stage1" && stage1Sel == true){
            stage1Sel = false;
            levels.Remove("Lab");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material1;
            }
        } 

        if (this.name == "stage2" && stage2Sel == false){
            stage2Sel = true;
            levels.Add("TrafficMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material2;
            }
        } else if (this.name == "stage2" && stage2Sel == true){
            stage2Sel = false;
            levels.Remove("TrafficMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material2;
            }
        }

        if (this.name == "stage3" && stage3Sel == false){
            stage3Sel = true;
            levels.Add("ConstructionMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material3;
            }
        } else if (this.name == "stage3" && stage3Sel == true){
            stage3Sel = false;
            levels.Remove("ConstructionMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material3;
            }
        }

        if (this.name == "stage4" && stage4Sel == false){
            stage4Sel = true;
            levels.Add("VolcanoMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material4;
            }
        } else if (this.name == "stage4" && stage4Sel == true){
            stage4Sel = false;
            levels.Remove("VolcanoMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material4;
            }
        }

        if (this.name == "stage5" && stage5Sel == false){
            stage5Sel = true;
            levels.Add("MallMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material5;
            }
        } else if (this.name == "stage5" && stage5Sel == true){
            stage5Sel = false;
            levels.Remove("MallMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material5;
            }
        }

        if (this.name == "stage6" && stage6Sel == false){
            stage6Sel = true;
            levels.Add("BalloonMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material6;
            }
        } else if (this.name == "stage6" && stage6Sel == true){
            stage6Sel = false;
            levels.Remove("BalloonMap");
            foreach( var x in levels) {
                //Debug.Log( x.ToString());
                thumbNail.GetComponent<Image>().material = Material6;
            }
        }
    }
}
