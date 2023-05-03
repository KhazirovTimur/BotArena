using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Interface for all projectiles

public interface IProjectile
{
    
    public void SetDamage(float damage);
    public void SetSpeed(float speed);
    public void ResetLifeTime();
}
