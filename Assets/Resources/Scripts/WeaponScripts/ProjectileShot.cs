using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShot : MonoBehaviour, IShootMechanic
{
    [Header("Projectile settings")]
    [Tooltip("If raycast is false, fill projectile here")]
    [SerializeField] protected GameObject projectile;
    [Tooltip("Speed of projectile")]
    [SerializeField] protected float projectileSpeed;
    [SerializeField] private LayerMask occlusionLayers;
    [Header("Hit effect reference")]
    [SerializeField] private GameObject hitEffect;
    
    //cache for projectiles pool
    private ObjectPoolContainer projectilePoolContainer;
    private ObjectPoolContainer _hitEffectsPool;

    private AbstractWeapon thisWeapon;

    private void Start()
    {
        thisWeapon = GetComponent<AbstractWeapon>();
        projectilePoolContainer = FindObjectOfType<AllObjectPoolsContainer>().
            CreateNewPool(projectile.GetComponent<IPoolable>(), thisWeapon.GetDefaultPoolCapacity());
        _hitEffectsPool = FindObjectOfType<AllObjectPoolsContainer>()
            .CreateNewPool(hitEffect.GetComponent<IPoolable>(), thisWeapon.GetDefaultPoolCapacity());
    }

    public void DoShot(Transform barrelEnd, float damage)
    {
        IPoolable newBullet = projectilePoolContainer.GetPool.Get();
        newBullet.GetGameObject().transform.position = barrelEnd.position;
        newBullet.GetGameObject().transform.rotation = barrelEnd.rotation;
        IProjectile bullet = newBullet.GetGameObject().GetComponent<IProjectile>();
        bullet.SetDamage(damage);
        bullet.SetSpeed(projectileSpeed);
        bullet.ResetItem();
        bullet.SetOcclusionLayers(occlusionLayers);
        if(!bullet.HaveHitEffectPool())
            bullet.SetHitEffectsPool(_hitEffectsPool);
    }
    
    public void DoCloseShot(Transform cameraRoot, float damage)
    {
        if (Physics.Raycast(cameraRoot.transform.position, cameraRoot.forward,
                out RaycastHit hit, 500, occlusionLayers))
        {
            IPoolable bulletHole = _hitEffectsPool.GetPool.Get();
            bulletHole.GetGameObject().GetComponent<HitDecals>().SetPosAndRotation(hit);
            if (hit.transform.TryGetComponent<IDamagable>(out IDamagable target))
            {
                target.TakeDamage(damage);
            }
        }
    }
    
}
