using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BodyPart", menuName = "Creature/BodyPart")]
public class BodyPartData : ScriptableObject
{
    public string part_name;
    public Image part_image;

    // Body Part stats.
    // default value = 10.
    // set values to 0 if it's not used by the part.
    [System.Serializable]
    public class Stats
    {
        public float health;
        public float strength; //arm grab strength, arm hit strength, leg jump height, turn speed? leg movespeed?
        public float mass;
        public float springConstant; //for arms only
    }

    public Stats stats;
}
