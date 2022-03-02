using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] private Transform skeletonBase;
    public BodyPartData partData;

    [Header("Part Stat Multipliers")]
    private float mass;
    private float healthMultiplier;
    private float strengthMultiplier;
    private float springConstantMultiplier;

    //STORAGE
    //public enum PartType { Head, Torso, Arm, Leg }
    //public PartType partType;
    //public enum PartSide { None, Left, Right }
    //public PartSide partSide;

    public void Awake()
    {
        ToggleKinematics(skeletonBase, true);

        if (partData != null)
        {
            healthMultiplier = partData.stats.healthMultiplier;
            mass = partData.stats.massMultiplier;
            strengthMultiplier = partData.stats.strengthMultiplier;
            springConstantMultiplier = partData.stats.springConstantMultiplier;
        }
    }

    public void Start()
    {
        /*
        if (partData != null)
        {
            healthMultiplier = partData.stats.healthMultiplier;
            mass = partData.stats.massMultiplier;
            strengthMultiplier = partData.stats.strengthMultiplier;
            springConstantMultiplier = partData.stats.springConstantMultiplier;
            print(mass);
        }
        */
    }

    public void ToggleKinematics(Transform currChild, bool toggleType)
    {
        if (currChild == null)
        {
            //return;
            currChild = gameObject.transform;
        }
        if (currChild.GetComponent<Rigidbody>() != null)
        {
            currChild.GetComponent<Rigidbody>().isKinematic = toggleType;
            //print("gary");
        }
        if (currChild.childCount > 0)
        {
            for (int i = 0; i < currChild.childCount; i++)
            {
                ToggleKinematics(currChild.GetChild(i), toggleType);
            }
        }
    }

    #region Getters
    public float getHealthMultiplier()
    {
        return healthMultiplier;
    }

    public float getMass()
    {
        return mass;
    }

    public float getStrengthMultiplier()
    {
        return strengthMultiplier;
    }

    public float getSpringConstantMultiplier()
    {
        return springConstantMultiplier;
    }
    #endregion

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