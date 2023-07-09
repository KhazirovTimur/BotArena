using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelDroneChaseState : AbstractState<WheelDroneStates>
{
    
    private float _checkPlayerPosTime = 0.5f;
    private float _checkPlayerPosTimer;

    private AttackComponent _attackComponent;
    private Transform _player;
    private EnemyAgent _thisAgent;

    private bool moveToPlayer;

    public WheelDroneChaseState(WheelDroneStateMachine stateMachine, WheelDroneStates thisState) 
        : base(stateMachine,  thisState){}

    public override void OnEnter()
    {
        _attackComponent = _stateMachine.GetThisEnemy.GetAttackComponent;
        _attackComponent.CanAttack += StopMoveToPlayer;
        _attackComponent.CanNotAttack += StartMoveToPlayer;
        _player = _stateMachine.GetThisEnemy.GetPlayerTransform;
        _thisAgent = _stateMachine.GetThisEnemy.GetThisNavAgent;
        _attackComponent.StartAttackOnReady();
    }

    public override void Updater()
    {
        if (moveToPlayer && _checkPlayerPosTimer < Time.time)
        {
            _thisAgent.MoveToPoint(_player.position);
            _checkPlayerPosTimer = Time.time + _checkPlayerPosTime;
        }
    }

    private void StartMoveToPlayer()
    {
        moveToPlayer = true;
    }

    private void StopMoveToPlayer()
    {
        moveToPlayer = false;
        _thisAgent.Stop();
    }

    public override void OnExit()
    {
        if(_attackComponent)
            _attackComponent.StopAllAttacks();
        _attackComponent.CanAttack -= StopMoveToPlayer;
        _attackComponent.CanNotAttack -= StartMoveToPlayer;
    }


}
