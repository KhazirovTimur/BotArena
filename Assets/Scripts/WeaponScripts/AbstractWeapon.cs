using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;




public abstract class AbstractWeapon : MonoBehaviour
{
    
    
    [FormerlySerializedAs("Name")] [Tooltip("Weapon name")]
    [SerializeField]
    protected string weaponName;
    public string GetWeaponName => weaponName;
    
    [Space(10)]
    [Header("Weapon stats")]
    [SerializeField]
    protected float Damage;
    [Tooltip("Fire rate, bullets pet minute")]
    [SerializeField]
    protected float rateOfFire;
    [Tooltip("Angle, to which bullets can deviate from central raycast")]
    [SerializeField]
    protected float SpreadAngle;
    [Tooltip("If true, weapon will shoot while hold fire button")]
    [SerializeField]
    protected bool IsFullAuto;

    [Tooltip("Attach projectile or raycast shooting mechanic")] 
    [SerializeField]
    protected IShootMechanic shootMechanic;
    
    
    [Space(10)]
    [Header("Other settings")]
    [Tooltip("Transform of empty gameobject at the end of barrel. Bullets spawn on this transform")]
    [SerializeField]
    protected Transform barrelEnd;
    [SerializeField]
    protected AmmoTypes.Ammotypes weaponAmmoType;
    public AmmoTypes.Ammotypes GetWeaponAmmoType => weaponAmmoType;
    
    //[SerializeField]
   // private ParticleSystem muzzleFlash;

    //Action for recoil
    public Action ShotWasMade;

    //variables for trigger behavior
    protected bool _triggerIsPushed;
    protected bool _triggerWasReleased;
    
    //changeable variables for weapon state
    protected float _delay;

    //cache for player inventory to update amount of ammo
    protected PlayerInventory _playerInventory;
    
    //Where player is aiming
    protected Vector3 _aim;

    
    
    

    
    protected void Start()
    {
        _triggerIsPushed = false;
        _triggerWasReleased = true;
        shootMechanic = GetComponent<IShootMechanic>();
        _playerInventory = FindObjectOfType<PlayerInventory>();
       // muzzleFlash.transform.position = barrelEnd.transform.position;
    }


    protected virtual void Update()
    {
        if (!_triggerIsPushed)
            _triggerWasReleased = true;
        if (ShotRequirements())
        {
            Shoot();
        }
        
    }

    //Check requirements to make shot
    protected virtual bool ShotRequirements()
    {
        if (!_triggerIsPushed)
            return false;
        if (!(Time.time > _delay))
            return false;
        if (!(IsFullAuto || _triggerWasReleased))
            return false;
        if (_playerInventory.GetAmmo((int)weaponAmmoType) <= 0)
            return false;
        if (!_playerInventory.IfCanShoot)
            return false;
        return true;
    }


    protected virtual void Shoot()
    {
       // muzzleFlash.Play();
        _triggerWasReleased = false;
        _playerInventory.ReduceAmmoByShot();
        barrelEnd.LookAt(_aim);
        RanomizeSpread();
        shootMechanic.DoShot(barrelEnd, Damage);
        _delay = Time.time + (60.0f/rateOfFire);
        barrelEnd.LookAt(_aim);
        ShotWasMade();
        
    }



    protected virtual void RanomizeSpread()
    {
        barrelEnd.localRotation =  Quaternion.Euler(
            barrelEnd.localRotation.eulerAngles.x + Random.Range(-SpreadAngle, SpreadAngle),
            barrelEnd.localRotation.eulerAngles.y + +Random.Range(-SpreadAngle, SpreadAngle),
            barrelEnd.localRotation.eulerAngles.z);
    }

    
    public void TriggerPushed(bool triggerState, Vector3 pointOnTarget)
    {
        _triggerIsPushed = triggerState;
        _aim = pointOnTarget;
    }


    public int GetDefaultPoolCapacity()
    {
        return (int)(rateOfFire / 60) * 2;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

}
