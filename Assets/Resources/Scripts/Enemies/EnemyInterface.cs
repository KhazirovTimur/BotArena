using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInterface : MonoBehaviour
{
    private AttackPlayer _attackPlayer;
    public AttackPlayer GetAttackPlayer => _attackPlayer;

    
    private EnemiesAgressionController _agressionController;
    public EnemiesAgressionController GetAgressionController => _agressionController;

    private EnemyAgent _thisAgent;
    public EnemyAgent GetThisNavAgent => _thisAgent;

    private SimpleEnemy _thisEnemyStats;
    public SimpleEnemy GetSimpleEnemy => _thisEnemyStats;

    private IStateMachineToDeath _thisStateMachine;
    public IStateMachineToDeath GetToDeathStateMachine => _thisStateMachine;

    private void Start()
    {
        _attackPlayer = GetComponent<AttackPlayer>();
        _thisAgent = GetComponent<EnemyAgent>();
        _thisEnemyStats = GetComponent<SimpleEnemy>();
        _thisStateMachine = GetComponent<IStateMachineToDeath>();
        _agressionController = FindObjectOfType<EnemiesAgressionController>();
        
        
    }


    void Update()
    {
        
    }
}
