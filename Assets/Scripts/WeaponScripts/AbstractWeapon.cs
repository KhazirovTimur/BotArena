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

    [Tooltip("If true, use raycast to detect hits, instead of projectiles")] 
    [SerializeField]
    protected IShootMechanic shootMechanic;
    
    
    [Space(10)]
    [Header("Other settings")]
    [Tooltip("Transform of empty gameobject at the end of barrel. Bullets spawn on this transform")]
    [SerializeField]
    protected Transform BarrelEnd;
    [SerializeField]
    protected AmmoTypes.Ammotypes weaponAmmoType;
    public AmmoTypes.Ammotypes GetWeaponAmmoType => weaponAmmoType;

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


    
    

    
    void Start()
    {
        _triggerIsPushed = false;
        _triggerWasReleased = true;
        shootMechanic = GetComponent<IShootMechanic>();
        _playerInventory = FindObjectOfType<PlayerInventory>();
    }

    //Check requirements to make shot
    protected virtual void Update()
    {
        if (!_triggerIsPushed)
            _triggerWasReleased = true;
        if (ShotRequirements())
        {
            Shoot();
        }
        
    }

    
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
        _triggerWasReleased = false;
        _playerInventory.ReduceAmmoByShot();
        BarrelEnd.LookAt(_aim);
        RanomizeSpread();
        shootMechanic.DoShot(BarrelEnd, Damage);
        _delay = Time.time + (60.0f/rateOfFire);
        BarrelEnd.LookAt(_aim);
        ShotWasMade();
    }



    protected virtual void RanomizeSpread()
    {
        BarrelEnd.localRotation =  Quaternion.Euler(
            BarrelEnd.localRotation.eulerAngles.x + Random.Range(-SpreadAngle, SpreadAngle),
            BarrelEnd.localRotation.eulerAngles.y + +Random.Range(-SpreadAngle, SpreadAngle),
            BarrelEnd.localRotation.eulerAngles.z);
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
