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
    [SerializeField] protected bool usePoolForProjectiles = true;
    
    //cache for projectiles pool
    private ObjectPoolContainer projectilePoolContainer;


    protected override void Initialize()
    {
        thisWeapon = GetComponent<AbstractWeapon>();
        if(usePoolForProjectiles)
            projectilePoolContainer = FindObjectOfType<AllObjectPoolsContainer>().
            CreateNewPool(projectile.GetComponent<IPoolable>(), thisWeapon.GetDefaultPoolCapacity());
        if(usePoolForHitEffects)
            _hitEffectsPool = FindObjectOfType<AllObjectPoolsContainer>()
            .CreateNewPool(hitEffect.GetComponent<IPoolable>(), thisWeapon.GetDefaultPoolCapacity());
    }

    public override void DoShot(Transform barrelEnd, float damage)
    {
        GameObject newBullet = GetProgectile();
        newBullet.transform.position = barrelEnd.position;
        newBullet.transform.rotation = barrelEnd.rotation;
        DoProjectileShot(barrelEnd, damage, newBullet.GetComponent<IProjectile>(), projectileSpeed);
    }

    private GameObject GetProgectile()
    {
        if (usePoolForProjectiles)
            return projectilePoolContainer.GetPool.Get().GetGameObject();
        return Instantiate(projectile);
    }

    public override void DoCloseShot(Transform cameraRoot, float damage)
    {
        base.DoRaycastShot(cameraRoot, damage);
    }

    public float GetDamageReducedByDistanceProjectile(float distance, float damage)
    {
        return GetReducedDamageByDistance(distance, damage);
    }

}
