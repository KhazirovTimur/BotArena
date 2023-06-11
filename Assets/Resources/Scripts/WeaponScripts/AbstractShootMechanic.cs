using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractShootMechanic : MonoBehaviour
{
    [SerializeField] protected LayerMask occlusionLayers;
    [SerializeField] protected GameObject hitEffect;
    protected ObjectPoolContainer _hitEffectsPool;
    protected AbstractWeapon thisWeapon;
    protected AnimationCurve damageDropOff;

    public Action<RaycastHit> HitTarget;
    
    protected virtual void Start()
    {
        thisWeapon = GetComponent<AbstractWeapon>();
        _hitEffectsPool = FindObjectOfType<AllObjectPoolsContainer>()
            .CreateNewPool(hitEffect.GetComponent<IPoolable>(), thisWeapon.GetDefaultPoolCapacity());
    }
    
    public virtual void DoShot(Transform barrelEnd, float damage)
    {
        if (Physics.Raycast(barrelEnd.transform.position, barrelEnd.forward,
                out RaycastHit hit, 500, occlusionLayers))
        {
            IPoolable bulletHole = _hitEffectsPool.GetPool.Get();
            bulletHole.GetGameObject().GetComponent<HitDecals>().SetPosAndRotation(hit);
            //HitTarget(hit);
            if (hit.transform.TryGetComponent<IDamagable>(out IDamagable target))
            {
                target.TakeDamage(GetReducedDamageByDistance(hit.distance, damage));
            }
        }
    }

    public virtual void DoCloseShot(Transform cameraRoot, float damage)
    {
        DoShot(cameraRoot, damage);
    }
    
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
