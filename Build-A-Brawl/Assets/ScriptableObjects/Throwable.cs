using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Throwable Object", menuName = "Throwable Object")]
public class Throwable : ScriptableObject
{
    [SerializeField] private new string name = "Throwable Name";
    [SerializeField] public bool pickedUp;

    public string Name => name;
    public bool PickedUp => pickedUp;

}