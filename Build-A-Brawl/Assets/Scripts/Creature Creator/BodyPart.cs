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
    private BodyPartData.animType attackTypeL;
    private BodyPartData.animType attackTypeR;

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

    public BodyPartData.animType getAttackTypeL()
    {
        return attackTypeL;
    }

    public BodyPartData.animType getAttackTypeR()
    {
        return attackTypeR;
    }
    #endregion

}