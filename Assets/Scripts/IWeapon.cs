using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void Shoot();

    void Reload();

    public void TriggerPushed(bool triggerStatePushed, Vector3 pointOnTarget);

}
