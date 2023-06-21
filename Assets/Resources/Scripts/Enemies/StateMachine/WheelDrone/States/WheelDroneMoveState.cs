using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelDroneMoveState : AbstractState<WheelDroneStates>
{
    
    private float cantAttackTimer = 3;
    private float _timer;
    
    public WheelDroneMoveState(WheelDroneStateMachine stateMachine, WheelDroneStates thisState) 
        : base(stateMachine,  thisState){}

    public override void OnEnter()
    {
        _timer = 0;
    }

    public override void Update()
    {
        if (_stateMachine.GetThisEnemy.GetAttackPlayer.CanShoot())
            _timer += Time.deltaTime;
        else
        {
            _timer = 0;
        }
        
        if(_timer > cantAttackTimer)
            _stateMachine.ChangeState(WheelDroneStates.Attack);
        
        if( _stateMachine.GetThisEnemy.GetThisNavAgent.CanCheckPlayerPos())
            Move();
    }

    private void Move()
    {
        _stateMachine.GetThisEnemy.GetThisNavAgent.MoveToPlayer();
    }

    public override void OnExit()
    {
        _stateMachine.GetThisEnemy.GetThisNavAgent.agent.ResetPath();
    }
}
