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
    
    
    [Tooltip("Weapon name")]
    [SerializeField]
    protected string weaponName;
    public string GetWeaponName => weaponName;

    [SerializeField] protected WeaponsEnums.Weapons weapon;
    public WeaponsEnums.Weapons GetWeapon => weapon;
    
    
    [Space(10)]
    [Header("Weapon stats")]
    [SerializeField] protected float Damage;
    [Tooltip("Fire rate, bullets pet minute")]
    [SerializeField] protected float rateOfFire;
    [Tooltip("Angle, to which bullets can deviate from central raycast")]
    [SerializeField] protected float SpreadAngle;
    [Tooltip("If true, weapon will shoot while hold fire button")]
    [SerializeField] protected bool IsFullAuto;
    [SerializeField] protected int ammoSpendPerShot = 1;
    [Tooltip("1 sec = 100m.")]
    [SerializeField] protected AnimationCurve damageDropOff;
    public AnimationCurve GetDamageDropOffCurve => damageDropOff;
    [SerializeField] protected WeaponsEnums.Ammotypes weaponAmmoType;
    public WeaponsEnums.Ammotypes GetWeaponAmmoType => weaponAmmoType; 
    
    
    [Space(10)]
    [Header("Other settings")]
    [Tooltip("Transform of empty gameobject at the end of barrel. Bullets spawn on this transform")]
    [SerializeField] protected Transform barrelEnd;
    [SerializeField] protected float minStandartShotDistance;
    [SerializeField] private List<GameObject> _muzzleVFXContainer = new List<GameObject>();
    
    [Space(10)]
    [Header("Sounds")]
    [SerializeField] protected AudioClip[] shotSounds;

    
    
    
    protected AbstractShootMechanic ShootMechanic;
    
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
    
    //Transform of empty gameobject for camera root. Used for shots at very close ranges
    protected Transform _playerCameraRootTransform;
    
    //FX
    private List<IMuzzleVFX> _muzzleVFX = new List<IMuzzleVFX>();
    protected AudioSource audioSource;
    

    
    public void InitializeWeapon()
    {
        _triggerIsPushed = false;
        _triggerWasReleased = true;
        ShootMechanic = GetComponent<AbstractShootMechanic>();
        _playerInventory = FindObjectOfType<PlayerInventory>();
        _playerCameraRootTransform = FindObjectOfType<CameraRootForShots>().transform;
        audioSource = GetComponent<AudioSource>();
        if(!_muzzleVFXContainer.IsUnityNull())
            foreach (var element in _muzzleVFXContainer)
            {
                _muzzleVFX.Add(element.GetComponent<IMuzzleVFX>());
            }
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
        if (_playerInventory.GetAmmo(weaponAmmoType) <= 0)
            return false;
        if (!_playerInventory.IfCanShoot)
            return false;
        return true;
    }


    protected virtual void Shoot()
    {
        _triggerWasReleased = false;
        _playerInventory.ReduceActiveWeaponAmmoByShot(ammoSpendPerShot);
        if (EnemyIsTooClose())
        {
            _playerCameraRootTransform.LookAt(_aim);
            RanomizeSpreadAngle(_playerCameraRootTransform);
            ShootMechanic.DoCloseShot(_playerCameraRootTransform, Damage);
            _playerCameraRootTransform.LookAt(_aim);
        }
        else
        {
            barrelEnd.LookAt(_aim);
            RanomizeSpreadAngle(barrelEnd);
            ShootMechanic.DoShot(barrelEnd, Damage);  
            barrelEnd.LookAt(_aim);
        }
        _delay = Time.time + (60.0f/rateOfFire);
        PlayShotEffects();
        ShotWasMade();
    }


    protected virtual void PlayShotEffects()
    {
        audioSource.PlayOneShot(shotSounds[Random.Range(0, shotSounds.Length)]);
        if (!_muzzleVFX.IsUnityNull())
        {
            foreach (var effect in _muzzleVFX)
            {
                effect.PlayVFX();
            }
        }
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
