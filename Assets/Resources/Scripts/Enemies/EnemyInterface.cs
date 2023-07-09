using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInterface : MonoBehaviour
{
    private AttackComponent _attackComponent;
    public AttackComponent GetAttackComponent => _attackComponent;
    
    private EnemiesAgressionController _agressionController;
    public EnemiesAgressionController GetAgressionController => _agressionController;

    private EnemyAgent _thisAgent;
    public EnemyAgent GetThisNavAgent => _thisAgent;

    private AbstractEnemyStatus _thisEnemyStatusStats;
    public AbstractEnemyStatus GetAbstractEnemyStatus => _thisEnemyStatusStats;

    private IStateMachineToDeath _thisStateMachine;
    public IStateMachineToDeath GetToDeathStateMachine => _thisStateMachine;

    private Transform _player;
    public Transform GetPlayerTransform => _player;

    private AbstractAnimation _thisAnim;
    public AbstractAnimation GetAnimation => _thisAnim;

    private PlayerPosPredictor _playerTracker;
    public PlayerPosPredictor GetPlayerPosPredictor => _playerTracker;

    private AudioSource _thisAudioSource;
    public AudioSource GetThisAudioSource => _thisAudioSource;



    private void Awake()
    {
        _attackComponent = GetComponent<AttackComponent>();
        _thisAgent = GetComponent<EnemyAgent>();
        _thisEnemyStatusStats = GetComponent<AbstractEnemyStatus>();
        _thisStateMachine = GetComponent<IStateMachineToDeath>();
        _thisAnim = GetComponent<AbstractAnimation>();
        _playerTracker = GetComponent<PlayerPosPredictor>();
        _thisAudioSource = GetComponent<AudioSource>();
        _agressionController = FindObjectOfType<EnemiesAgressionController>();
        _player = FindObjectOfType<FirstPersonController>().transform;


    }


    void Update()
    {
        
    }
}
