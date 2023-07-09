using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileShootingMechanic
{
    
    public IPoolable GetProjectileReference();

    public bool NeedPoolForProjectiles();

    public void SetProjectilePool(ObjectPoolContainer pool);
}
