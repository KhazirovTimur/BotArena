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

    public float GetProjectileSpeed => projectileSpeed;
    
    //cache for projectiles pool
    private ObjectPoolContainer _projectilePoolContainer;
    

    public override void DoShot(Transform barrelEnd, float damage)
    {
        GameObject newBullet = GetProjectile();
        newBullet.transform.position = barrelEnd.position;
        newBullet.transform.rotation = barrelEnd.rotation;
        DoProjectileShot(barrelEnd, damage, newBullet.GetComponent<IProjectile>(), projectileSpeed);
    }

    private GameObject GetProjectile()
    {
        if (usePoolForProjectiles)
            return _projectilePoolContainer.GetPool.Get().GetGameObject();
        return Instantiate(projectile);
    }

    public override void DoCloseShot(Transform cameraRoot, float damage)
    {
        base.DoRaycastShot(cameraRoot, damage);
    }
    

    public IPoolable GetProjectileReference()
    {
        return projectile.GetComponent<IPoolable>();
    }

    public bool NeedPoolForProjectiles()
    {
        return usePoolForProjectiles;
    }

    public void SetProjectilePool(ObjectPoolContainer pool)
    {
        _projectilePoolContainer = pool;
    }
}
