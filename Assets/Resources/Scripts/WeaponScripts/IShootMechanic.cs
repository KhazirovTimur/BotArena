using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootMechanic
{
    public void DoShot(Transform barrelEnd, float damage);
}
