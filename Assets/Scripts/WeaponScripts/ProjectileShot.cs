using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShot : MonoBehaviour, IShootMechanic
{
    [Header("Projectile settings")]
    [Tooltip("If raycast is false, fill projectile here")]
    [SerializeField]
    protected GameObject projectile;
    [Tooltip("Speed of projectile")]
    [SerializeField]
    protected float projectileSpeed;
    
    public LayerMask occlusionLayers;
    
    //cache for projectiles pool
    private ObjectPoolContainer projectilePoolContainer;

    private AbstractWeapon thisWeapon;

    private void Start()
    {
        thisWeapon = GetComponent<AbstractWeapon>();
        projectilePoolContainer = FindObjectOfType<AllObjectPoolsContainer>().
            CreateNewPool(projectile.GetComponent<IPoolable>(), thisWeapon.GetDefaultPoolCapacity());
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
    }
}
