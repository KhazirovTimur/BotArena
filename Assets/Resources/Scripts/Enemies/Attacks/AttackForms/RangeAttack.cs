using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;

public class RangeAttack : AbstractWeapon, IEnemyAttack
{
    [Space(10)]
    [Header("Enemy configs")]
    [SerializeField] private LayerMask environmentLayers;
    [SerializeField] private float maxAttackDistance = 15;

    private Vector3 _offset;
   
    private EnemyInterface _thisEnemy;
    public event IEnemyAttack.EventHandler FinishAttack;
    
    
    
    private void Start()
    {
       InitializeWeapon();
       CalculateOffset();
    }
    
    public override void InitializeWeapon()
    {
        _thisEnemy = GetComponentInParent<EnemyInterface>();
        ShootMechanic = GetComponent<AbstractShootMechanic>();
        audioSource = _thisEnemy.GetThisAudioSource;
        if(!_muzzleVFXContainer.IsUnityNull())
            foreach (var element in _muzzleVFXContainer)
            {
                _muzzleVFX.Add(element.GetComponent<IMuzzleVFX>());
            }
        InitPools();
    }
    
    protected override void Update()
    {
        _aim = GetAimingPoint();
    }
    
    protected override bool ShotRequirements()
    {
        if(Physics.Linecast(barrelEnd.position, AttackComponent.PlayerTransform.position + Vector3.up,  environmentLayers))
             return false;
        return true;
    }
    
    protected override void Shoot()
    {
        barrelEnd.LookAt(_aim);
        RanomizeSpreadAngle(barrelEnd);
        if (HitPointIsTooClose())
        {
            ShootMechanic.DoCloseShot(barrelEnd, Damage);
        }
        else
        {
            ShootMechanic.DoShot(barrelEnd, Damage);
        }
        barrelEnd.LookAt(_aim);
        PlayShotEffects();
        ShotWasMade?.Invoke();
        FinishAttack?.Invoke();
    }
    

    public float GetMaxAttackDistance()
    {
        return maxAttackDistance;
    }

    public void DoAttack()
    {
        Shoot();
    }

    public bool ConditionsMet()
    {
        return ShotRequirements();
    }

    public float GetCooldownTime()
    {
        return 60 / rateOfFire;
    }

    public Vector3 GetAimingPoint()
    {
        if (ShootMechanic.TryGetComponent(out ProjectileShot mechanic))
            return _thisEnemy.GetPlayerPosPredictor.PredictPlayerPosition(mechanic.GetProjectileSpeed) - _offset;
        return _thisEnemy.GetPlayerPosPredictor.PredictPlayerPosition(0) - _offset;
    }
    
    private void CalculateOffset()
    {
        _offset = _thisEnemy.transform.position - transform.position;
        _offset = new Vector3(_offset.x, 0, _offset.z);

    }

    protected override bool HitPointIsTooClose()
    {
        if ((_aim - barrelEnd.position).magnitude < minStandartShotDistance)
            return true;
        return false;
    }

    
}
