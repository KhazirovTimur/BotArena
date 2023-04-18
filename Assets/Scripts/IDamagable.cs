using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//If object can receive damage, must inherit from this

public interface IDamagable
{
    public void TakeDamage(float damage);
}
