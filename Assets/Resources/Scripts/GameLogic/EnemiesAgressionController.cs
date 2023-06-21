using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemiesAgressionController : MonoBehaviour
{

    [SerializeField] private int maxAttackers;

    [SerializeField] private float minAttackRate = 1;

    private List<IAttacker> _attackingEnemies = new List<IAttacker>();

    private List<IAttacker> _attackersQueue = new List<IAttacker>();

    private float minAttackRateTimer;

    public void TryGetPermissionToAttack(IAttacker enemy)
    {
        if(!_attackersQueue.Contains(enemy))
            _attackersQueue.Add(enemy);
    }

    private void Update()
    {
        if(_attackingEnemies.Count < maxAttackers && minAttackRateTimer < Time.time)
            GiveNewPermissions();
    }

    private void GiveNewPermissions()
    {
        minAttackRateTimer = Time.time + minAttackRate;
        if(_attackersQueue.Count == 0)
            return;
        _attackersQueue.Sort((a,b) => a.GetAttackPriority().CompareTo(b.GetAttackPriority()));
        while (_attackingEnemies.Count < maxAttackers && _attackersQueue.Count != 0)
        {
            _attackingEnemies.Add(_attackersQueue[0]);
            _attackersQueue[0].GivePermission();
            _attackersQueue.RemoveAt(0);
        }
    }

    public float CountAttackPriority(float distanceToPlayer, float attackPower)
    {
        return attackPower + (distanceToPlayer / 10);
    }

    public void ReleaseFromAttackers(IAttacker enemy)
    {
        _attackingEnemies.Remove(enemy);
    }
    
    public void RemoveFromAttackersQueue(IAttacker enemy)
    {
        _attackersQueue.Remove(enemy);
    }

}
