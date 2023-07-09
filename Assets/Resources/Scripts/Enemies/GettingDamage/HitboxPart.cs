using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxPart : MonoBehaviour, IDamagable
{
    [SerializeField] private float damageMultiplier = 1.0f;

    private AbstractEnemyStatus _thisEnemyStatus;
    
    private void Start()
    {
        _thisEnemyStatus = GetComponentInParent<AbstractEnemyStatus>();
    }

    public void TakeDamage(float damage)
    {
        _thisEnemyStatus.TakeDamage(damage * damageMultiplier);
    }
}
