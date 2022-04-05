using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BodyPart", menuName = "Creature/BodyPart")]
public class BodyPartData : ScriptableObject
{
    public string part_name;
    public Image part_image;
    public List<Sprite> symbols;

    [Header("Audio")]
    public List<AudioClip> funNoises;
    public List<AudioClip> hurtNoises;


    public enum animType
    {
        Default,
        Vines,
        Robot
    }

    // Body Part stats.
    // default value = 10.
    // set values to 0 if it's not used by the part.
    [System.Serializable]
    public class Stats
    {
        public float healthMultiplier;
        public float strengthMultiplier; //arm grab strength, arm hit strength, leg jump height, turn speed? leg movespeed?
        public float massMultiplier;
        public float springConstantMultiplier; //for arms only
        public animType animType;
    }

    public Stats stats;
}
