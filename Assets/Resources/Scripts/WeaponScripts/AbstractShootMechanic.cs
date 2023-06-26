using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractShootMechanic : MonoBehaviour
{
    [SerializeField] protected LayerMask occlusionLayers;
    [SerializeField] protected GameObject hitEffect;
    [SerializeField] protected float maxRaycastDistance = 500;
    protected ObjectPoolContainer _hitEffectsPool;
    protected AbstractWeapon thisWeapon;
    protected AnimationCurve damageDropOff;

    
    //Used for bullet holes
    public Action<Vector3> HitTarget;
    
    protected virtual void Start()
    {
        thisWeapon = GetComponent<AbstractWeapon>();
        _hitEffectsPool = FindObjectOfType<AllObjectPoolsContainer>()
            .CreateNewPool(hitEffect.GetComponent<IPoolable>(), thisWeapon.GetDefaultPoolCapacity());
    }
    
    protected virtual void DoRaycastShot(Transform barrelEnd, float damage)
    {
        if (Physics.Raycast(barrelEnd.transform.position, barrelEnd.forward,
                out RaycastHit hit, maxRaycastDistance, occlusionLayers))
        {
            IPoolable bulletHole = _hitEffectsPool.GetPool.Get();
            bulletHole.GetGameObject().GetComponent<HitDecals>().SetPosAndRotation(hit);
            HitTarget?.Invoke(hit.point);
            if (hit.transform.TryGetComponent<IDamagable>(out IDamagable target))
            {
                target.TakeDamage(GetReducedDamageByDistance(hit.distance, damage));
            }
        }
        else
        {
            HitTarget?.Invoke(barrelEnd.transform.position + barrelEnd.forward * maxRaycastDistance);
        }
    }
    

    protected virtual void DoProjectileShot(Transform barrelEnd, float damage, IProjectile bullet, float projectileSpeed)
    {
        bullet.SetParentShooter(this);
        bullet.SetDamage(damage);
        bullet.SetSpeed(projectileSpeed);
        bullet.ResetItem();
        bullet.SetOcclusionLayers(occlusionLayers);
        if(!bullet.CheckHitEffectPool())
            bullet.SetHitEffectsPool(_hitEffectsPool);
    }

    public abstract void DoShot(Transform barrelEnd, float damage);

    public abstract void DoCloseShot(Transform cameraRoot, float damage);
    
    
    public float GetReducedDamageByDistance(float distance, float damage)
    {
        distance = distance / 100;
        for(int i = 0; i < thisWeapon.GetDamageDropOffCurve.length; i++)
        { 
            if (distance < thisWeapon.GetDamageDropOffCurve.keys[i].time)
            {
                return damage * thisWeapon.GetDamageDropOffCurve.keys[i].value;
            }
        }
        return damage * thisWeapon.GetDamageDropOffCurve.keys[thisWeapon.GetDamageDropOffCurve.length - 1].value;
    }
}
