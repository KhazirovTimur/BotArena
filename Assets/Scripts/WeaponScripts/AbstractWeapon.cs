using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;




public abstract class AbstractWeapon : MonoBehaviour
{
    
    
    [Tooltip("Weapon name")]
    public string Name;
    [Space(10)]
    [Header("Weapon stats")]
    public float Damage;
    [Tooltip("Delay between shots in seconds (Fire rate)")]
    public float ShotDelay;
    [Tooltip("Angle, to which bullets can deviate from central raycast")]
    public float SpreadAngle;
    [Tooltip("If true, weapon will shoot while hold fire button")]
    public bool IsFullAuto;
    [Tooltip("If true, use raycast to detect hits, instead of projectiles")]
    public bool IsRayCast;

    
    [Space(10)]
    [Header("Projectile settings")]
    [Tooltip("If raycast is false, fill projectile here")]
    public GameObject Projectile;
    [Tooltip("Speed of projectile")]
    public float ProjectileSpeed;
    
    
    [Space(10)]
    [Header("Other settings")]
    [Tooltip("Transform of empty gameobject at the end of barrel. Bullets spawn on this transform")]
    public Transform BarrelEnd;
    public AmmoTypes.Ammotypes WeaponAmmoType;

    //Action for recoil
    public Action ShotWasMade;
   

    //cache for projectiles pool
    [HideInInspector]
    public string ProjectilePoolIndex;
    //variables for trigger behavior
    protected bool _triggerIsPushed;
    protected bool _triggerWasReleased;
    
    //changeable variables for weapon state
    protected float _delay;

    //cache for player inventory to update amount of ammo
    protected PlayerInventory _playerInventory;
    
    //Where player is aiming
    protected Vector3 _aim;
    //Gameobject, which was hit with raycast
    protected RaycastHit _target;

    
    

    
    void Start()
    {
        ProjectilePoolIndex = Name;
        _triggerIsPushed = false;
        _triggerWasReleased = true;
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
        if(!(Time.time > _delay))
            return false;
        if (!(IsFullAuto || _triggerWasReleased))
            return false;
        if (_playerInventory.GetAmmo((int)WeaponAmmoType) <= 0)
            return false;
        return true;
    }


    protected virtual void Shoot()
    {
        _triggerWasReleased = false;
        _playerInventory.ReduceAmmoByOne();
        BarrelEnd.LookAt(_aim);
        //adding spread
        RanomizeSpread();
        if (!IsRayCast)
        {
            ProjectileShot();
        }
        else
        {
            RaycastShot();
        }
        _delay = Time.time + ShotDelay;
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


    protected virtual void ProjectileShot()
    {
        IPoolable newBullet = _playerInventory.Pooler.GetPool(ProjectilePoolIndex).Get();
        newBullet.GetGameObject().transform.position = BarrelEnd.position;
        newBullet.GetGameObject().transform.rotation = BarrelEnd.rotation;
        IProjectile bullet = newBullet.GetGameObject().GetComponent<IProjectile>();
        bullet.SetDamage(Damage);
        bullet.SetSpeed(ProjectileSpeed);
        bullet.ResetLifeTime();
    }

    protected virtual void RaycastShot()
    {
        if (Physics.Raycast(transform.position, BarrelEnd.forward,
                out _target, 1000))
        {
            if (_target.transform.TryGetComponent<IDamagable>(out IDamagable target))
            {
                target.TakeDamage(Damage);
            }
        }
    }



    //Get input 
    public void TriggerPushed(bool triggerStatePushed, Vector3 pointOnTarget)
    {
        _triggerIsPushed = triggerStatePushed;
        _aim = pointOnTarget;
    }



}
