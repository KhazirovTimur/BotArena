using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractShootMechanic : MonoBehaviour
{
    [SerializeField] protected LayerMask occlusionLayers;
    public LayerMask GetOcclusionLayers => occlusionLayers;
    [SerializeField] protected GameObject hitEffect;
    [SerializeField] protected bool usePoolForHitEffects = true;
    [SerializeField] protected float maxRaycastDistance = 500;
    protected ObjectPoolContainer _hitEffectsPool;
    protected AbstractWeapon thisWeapon;
    protected AnimationCurve damageDropOff;

    
    //Used for muzzle line render
    public Action<Vector3> HitTarget;
    

    protected virtual void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        if(TryGetComponent(out AbstractWeapon weapon))
            thisWeapon = weapon;
        if(usePoolForHitEffects)
        {
            _hitEffectsPool = FindObjectOfType<AllObjectPoolsContainer>()
                .CreateNewPool(hitEffect.GetComponent<IPoolable>(), thisWeapon.GetDefaultPoolCapacity());
        }
        if(thisWeapon)
            damageDropOff = thisWeapon.GetDamageDropOffCurve;
    }

    protected virtual void DoRaycastShot(Transform barrelEnd, float damage)
    {
        if (Physics.Raycast(barrelEnd.transform.position, barrelEnd.forward,
                out RaycastHit hit, maxRaycastDistance, occlusionLayers))
        {
            GameObject bulletHole = GetHitEffect();
            bulletHole.GetComponent<IHitEffect>().SetPosAndRotation(hit);
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
    }

    public abstract void DoShot(Transform barrelEnd, float damage);

    public abstract void DoCloseShot(Transform cameraRoot, float damage);

    public GameObject GetHitEffect()
    {
        if(usePoolForHitEffects)
            return _hitEffectsPool.GetPool.Get().GetGameObject();
        return Instantiate(hitEffect);
    }
    
    
    public float GetReducedDamageByDistance(float distance, float damage)
    {
        distance = distance / 100;
        for(int i = 0; i < damageDropOff.length; i++)
        { 
            if (distance < damageDropOff.keys[i].time)
            {
                return damage * damageDropOff.keys[i].value;
            }
        }
        return damage * damageDropOff.keys[damageDropOff.length - 1].value;
    }
}
