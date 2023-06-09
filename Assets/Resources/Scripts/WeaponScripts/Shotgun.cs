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
      if (EnemyIsTooClose())
      {
         for (int i = 0; i < ProjectilesPerShot; i++)
         {
            _playerCameraRootTransform.LookAt(_aim);
            RanomizeSpreadAngle(_playerCameraRootTransform);
            shootMechanic.DoCloseShot(_playerCameraRootTransform, Damage);
            _playerCameraRootTransform.LookAt(_aim);
         }
      }
      else
      {
         for (int i = 0; i < ProjectilesPerShot; i++)
         {
            barrelEnd.LookAt(_aim);
            RanomizeSpreadAngle(barrelEnd);
            shootMechanic.DoShot(barrelEnd, Damage);  
            barrelEnd.LookAt(_aim);
         }
      }
      _delay = Time.time + (60 / rateOfFire);
      barrelEnd.LookAt(_aim);
      ShotWasMade();
      PlayShotEffects();
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
