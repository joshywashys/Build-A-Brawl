using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNewHazard : HazardObject
{
	protected override void Start()
    {
        base.Start();
        onDisplayHazard(gameObject);
    }
}
