using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileShootingMechanic
{
    public float GetDamageReducedByDistanceProjectile(float distance, float damage);
}
