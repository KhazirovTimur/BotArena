using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShot : AbstractShootMechanic, IProjectileShootingMechanic
{
    [Header("Projectile settings")]
    [SerializeField] protected GameObject projectile;
    [Tooltip("Speed of projectile")]
    [SerializeField] protected float projectileSpeed;
    
    //cache for projectiles pool
    private ObjectPoolContainer projectilePoolContainer;


    protected override void Start()
    {
        thisWeapon = GetComponent<AbstractWeapon>();
        projectilePoolContainer = FindObjectOfType<AllObjectPoolsContainer>().
            CreateNewPool(projectile.GetComponent<IPoolable>(), thisWeapon.GetDefaultPoolCapacity());
        _hitEffectsPool = FindObjectOfType<AllObjectPoolsContainer>()
            .CreateNewPool(hitEffect.GetComponent<IPoolable>(), thisWeapon.GetDefaultPoolCapacity());
    }

    public override void DoShot(Transform barrelEnd, float damage)
    {
        IPoolable newBullet = projectilePoolContainer.GetPool.Get();
        newBullet.GetGameObject().transform.position = barrelEnd.position;
        newBullet.GetGameObject().transform.rotation = barrelEnd.rotation;
        IProjectile bullet = newBullet.GetGameObject().GetComponent<IProjectile>();
        bullet.SetParentShooter(this);
        bullet.SetDamage(damage);
        bullet.SetSpeed(projectileSpeed);
        bullet.ResetItem();
        bullet.SetOcclusionLayers(occlusionLayers);
        if(!bullet.HaveHitEffectPool())
            bullet.SetHitEffectsPool(_hitEffectsPool);
    }
    
    public override void DoCloseShot(Transform cameraRoot, float damage)
    {
        base.DoShot(cameraRoot, damage);
    }

    public float GetDamageReducedByDistanceProjectile(float distance, float damage)
    {
        return GetReducedDamageByDistance(distance, damage);
    }

}
