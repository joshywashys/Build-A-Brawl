using UnityEngine;
using UnityEngine.UI;

//todo:
//use tranforms that respresent joint links
//neck, shoulder, hip connections for torso as transforms

/*
HOW/WHEN TO USE:
Make sure to only use Head/Torso/Arms/Legs child classes.
These scripts generate data for the creature creator to coordinate the parts together, and contain stats and modifiers for each part's gameplay.
Only attach those scripts to an object once:
-it has empty children gameObjects respresenting where each of it's joints are.
-make sure that the creature's left and right joints are placed on the correct sides: the creature will always be facing positive Z, and the left/right joints are the creature's left/right.

INDEXES (what order to put the joints in inside the unity editor):
0 neckJoint
1 shoulderJointL
2 shoulderJointR
3 hipJointL
4 hipJointR
*/

public abstract class BodyPart : MonoBehaviour
{
    private float health;
    private float mass;

    //STORAGE
    public enum PartType { Head, Torso, Arm, Leg }
    public PartType partType;

    public BodyPartData partData;
    public enum PartSide { None, Left, Right }
    public PartSide partSide;

}

[CreateAssetMenu(fileName = "BodyPart", menuName = "Creature/BodyPart")]
public class BodyPartData : ScriptableObject
{
    public string part_name;
    public Image part_image;

    // Body Part stats. set values to -1 (or dont bother setting them) if it's not used by the part (ie head has no strength).
    [System.Serializable]
    public struct Stats
    {
        public float health; 
        public float strength; //also affects how fast the player can turn, not just attack power
        public float mass; 
    }

    public Stats stats;
}