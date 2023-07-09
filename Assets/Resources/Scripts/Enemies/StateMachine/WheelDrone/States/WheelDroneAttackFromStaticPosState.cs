using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelDroneAttackFromStaticPosState : AbstractState<WheelDroneStates>
{
    
    private Vector3 _destination;
    private bool _destinationWasReached;
    private AttackComponent _attackComponent;
    
    
    public WheelDroneAttackFromStaticPosState(WheelDroneStateMachine stateMachine, WheelDroneStates thisState) 
        : base(stateMachine,  thisState){}

    public override void OnEnter()
    {
        _attackComponent = _stateMachine.GetThisEnemy.GetAttackComponent;
        if (_stateMachine.GetThisEnemy.GetAbstractEnemyStatus.GetPositionsController.HavePositions())
        {
            _stateMachine.GetThisEnemy.GetAbstractEnemyStatus.SetCurrentStaticPosition(_stateMachine.GetThisEnemy
                    .GetAbstractEnemyStatus.GetPositionsController.GetAvailablePosition());
            _stateMachine.GetThisEnemy.GetAbstractEnemyStatus.GetCurrentStaticPosition.OccupyPosition();
            _destination = _stateMachine.GetThisEnemy.GetAbstractEnemyStatus.GetCurrentStaticPosition.transform.position;
            _stateMachine.GetThisEnemy.GetThisNavAgent.MoveToPoint(_destination);
        }
        else
        {
            _stateMachine.ChangeState(WheelDroneStates.Chase);
        }
    }

    public override void Updater()
    {
        if ((_stateMachine.transform.position - _destination).magnitude < 1 && !_destinationWasReached)
        {
            _attackComponent.StartAttackOnReady();
            _destinationWasReached = true;
        }
    }
    

    public override void OnExit()
    {
        if(_stateMachine.GetThisEnemy.GetAbstractEnemyStatus.GetCurrentStaticPosition)
            _stateMachine.GetThisEnemy.GetAbstractEnemyStatus.GetCurrentStaticPosition.FreePosition();
        if(_attackComponent)
            _attackComponent.StopAllAttacks();
    }
}
