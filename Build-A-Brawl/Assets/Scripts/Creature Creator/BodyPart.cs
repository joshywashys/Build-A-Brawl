using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] public Transform skeletonBase;
    private Component[] colliderJoints;
    public BodyPartData partData;
    public CreatureStats creature;

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

        colliderJoints = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in colliderJoints)
        {
            if(rb.gameObject.GetComponent<JointCollision>() == null)
            {
                JointCollision jc = rb.gameObject.AddComponent<JointCollision>();
                jc.bodyPart = gameObject.GetComponent<BodyPart>();

                foreach (Collider col in rb.gameObject.GetComponents<Collider>())
                {
                    col.enabled = true;
                }
            }
        }

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
        if (gameObject.name == "torso")
        {
            creature = transform.parent.GetComponent<CreatureStats>();
        }
        if (gameObject.name == "head" || gameObject.name == "armL" || gameObject.name == "armR" || gameObject.name == "legs")
        {
            creature = transform.parent.parent.GetComponent<CreatureStats>();
        }
    }

    /*
    private void SetupColliders(Transform currChild)
    {
        if (currChild == null)
        {
            //currChild = gameObject.transform;
        }
        if (currChild.GetComponent<Rigidbody>() != null)
        {
            currChild.gameObject.AddComponent<JointCollision>();
        }
        if (currChild.childCount > 0)
        {
            for (int i = 0; i < currChild.childCount; i++)
            {
                SetupColliders(currChild.GetChild(i));
            }
        }
    }
    */

    public void ToggleKinematics(Transform currChild, bool toggleType)
    {
        if (currChild == null)
        {
            currChild = gameObject.transform;
        }
        if (currChild.GetComponent<Rigidbody>() != null)
        {
            currChild.GetComponent<Rigidbody>().isKinematic = toggleType;
            if (toggleType == false) { currChild.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; }
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