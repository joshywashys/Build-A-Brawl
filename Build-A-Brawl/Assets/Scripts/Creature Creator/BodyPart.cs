using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public BodyPartData partData;
    private float health;
    private float mass;
    private float strength;
    private float springConstant;

    //STORAGE
    //public enum PartType { Head, Torso, Arm, Leg }
    //public PartType partType;
    //public enum PartSide { None, Left, Right }
    //public PartSide partSide;

    public void Start()
    {
        health = partData.stats.health;
        mass = partData.stats.mass;
        strength = partData.stats.strength;
        springConstant = partData.stats.springConstant;
    }

}

/*
[CreateAssetMenu(fileName = "BodyPart", menuName = "Creature/BodyPart")]
public class BodyPartData : ScriptableObject
{
    public string part_name;
    public Image part_image;

    // Body Part stats.
    // default value = 10.
    // set values to 0 if it's not used by the part.
    [System.Serializable]
    public struct Stats
    {
        public float health; 
        public float strength; //arm grab strength, arm hit strength, leg jump height, turn speed? leg movespeed?
        public float mass;
        public float springConstant; //for arms only
    }

    public Stats stats;
}
*/