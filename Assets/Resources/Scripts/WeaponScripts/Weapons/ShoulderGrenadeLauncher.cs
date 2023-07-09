using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderGrenadeLauncher : AbstractWeapon
{
    private void Start()
    {
        InitializeWeapon();
    }

    protected override bool ShotRequirements()
    {
        if (!_triggerIsPushed)
            return false;
        if (!(Time.time > _delay))
            return false;
        if (!(isFullAuto || _triggerWasReleased))
            return false;
        return true;
    }
    
    protected override void Shoot()
    {
        _triggerWasReleased = false;
        if (HitPointIsTooClose())
        {
            ShootMechanic.DoCloseShot(transform, Damage);
        }
        else
        {
            ShootMechanic.DoShot(transform, Damage);
        }
        _delay = Time.time + (60.0f/rateOfFire);
        PlayShotEffects();
        ShotWasMade?.Invoke();
    }
    
    protected override bool HitPointIsTooClose()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,100, ShootMechanic.GetOcclusionLayers))
        {
            if ((hit.point - transform.position).magnitude < minStandartShotDistance)
                return true;
        }
        return false;
    }
    
}
