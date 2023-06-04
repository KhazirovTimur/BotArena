using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : AbstractWeapon
{
   [Space] [Header("Special shotgun parameters")]
   public int ProjectilesPerShot;

   protected override void Shoot()
   {
      _triggerWasReleased = false;
      _playerInventory.ReduceAmmoByShot();
      for (int i = 0; i < ProjectilesPerShot; i++)
      {
         barrelEnd.LookAt(_aim);
         RanomizeSpread();
         shootMechanic.DoShot(barrelEnd, Damage);
      }
      _delay = Time.time + (60 / rateOfFire);
      barrelEnd.LookAt(_aim);
      ShotWasMade();

   }

   protected override void Update()
   {
      if (!_triggerIsPushed)
         _triggerWasReleased = true;
      if (ShotRequirements())
      {
         Shoot();
      }
   }
}
