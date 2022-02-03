using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] private Transform skeletonBase;
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

    public void Awake()
    {
        ToggleKinematics(skeletonBase, true);
    }

    public void Start()
    {
        health = partData.stats.health;
        mass = partData.stats.mass;
        strength = partData.stats.strength;
        springConstant = partData.stats.springConstant;
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
    public float getHealth()
    {
        return health;
    }

    public float getMass()
    {
        return mass;
    }

    public float getStrength()
    {
        return mass;
    }

    public float getSpringConstant()
    {
        return springConstant;
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