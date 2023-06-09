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
    [SerializeField] protected Transform barrelEnd;

    //Transform of empty gameobject for camera root. Used for shots at very close ranges
    protected Transform _playerCameraRootTransform;
    [SerializeField] protected float minStandartShotDistance;
    
    
    [SerializeField] protected AmmoTypes.Ammotypes weaponAmmoType;
    public AmmoTypes.Ammotypes GetWeaponAmmoType => weaponAmmoType;
    
    //[SerializeField]
    //private ParticlesFXContainer muzzleFlash;

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
        _playerCameraRootTransform = FindObjectOfType<CameraRootForShots>().transform;
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
        _triggerWasReleased = false;
        _playerInventory.ReduceAmmoByShot();
        if (EnemyIsTooClose())
        {
            _playerCameraRootTransform.LookAt(_aim);
            RanomizeSpreadAngle(_playerCameraRootTransform);
            shootMechanic.DoCloseShot(_playerCameraRootTransform, Damage);
            _playerCameraRootTransform.LookAt(_aim);
        }
        else
        {
            barrelEnd.LookAt(_aim);
            RanomizeSpreadAngle(barrelEnd);
            shootMechanic.DoShot(barrelEnd, Damage);  
            barrelEnd.LookAt(_aim);
        }
        _delay = Time.time + (60.0f/rateOfFire);
        ShotWasMade();
    }


    protected virtual void PlayShotEffects()
    {
        //muzzleFlash.Play();
    }



    protected virtual void RanomizeSpreadAngle(Transform ToRandom)
    {
        ToRandom.localRotation =  Quaternion.Euler(
            ToRandom.localRotation.eulerAngles.x + Random.Range(-SpreadAngle, SpreadAngle),
            ToRandom.localRotation.eulerAngles.y + +Random.Range(-SpreadAngle, SpreadAngle),
            ToRandom.localRotation.eulerAngles.z);
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


    protected virtual bool EnemyIsTooClose()
    {
        if ((_aim - _playerCameraRootTransform.position).magnitude < (barrelEnd.position - _playerCameraRootTransform.position).magnitude)
            return true;
        if ((_aim - barrelEnd.position).magnitude < minStandartShotDistance)
            return true;
        return false;
    }


}
