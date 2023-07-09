using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDamageHitbox : MonoBehaviour, IDamagable
{

    private PlayerStatus _player;
    
    private void Start()
    {
        _player = GetComponentInParent<PlayerStatus>();
    }

    public void TakeDamage(float damage)
    {
        _player.TakeDamage(damage);
    }
}
