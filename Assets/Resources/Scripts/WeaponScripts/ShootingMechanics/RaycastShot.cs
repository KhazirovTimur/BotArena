using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaycastShot :  AbstractShootMechanic
{
    public override void DoShot(Transform barrelEnd, float damage)
    {
        DoRaycastShot(barrelEnd, damage);
    }

    public override void DoCloseShot(Transform cameraRoot, float damage)
    {
        DoRaycastShot(cameraRoot, damage);
    }
}
