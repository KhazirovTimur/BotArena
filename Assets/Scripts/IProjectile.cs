using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Interface for all projectiles

public interface IProjectile
{
    
    public void SetDamage(float damage);
    public void SetSpeed(float speed);

    
    //Is needed for object pool
    public void SetParentPool(ObjectPooler pooler, int index);
    public void ResetLifeTime();
}
