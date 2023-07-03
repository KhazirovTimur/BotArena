using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelDroneSpawnState : AbstractState<WheelDroneStates>
{

    private Vector3 _destination;
    
    public WheelDroneSpawnState(AbstractStateMachine<WheelDroneStates> stateMachine, WheelDroneStates thisState) : base(stateMachine, thisState)
    {
    }

    public override void OnEnter()
    {
        if (_stateMachine.GetThisEnemy.GetAbstractEnemy.GetPositionsController.HavePositions())
        {
            _destination = _stateMachine.GetThisEnemy.GetAbstractEnemy.GetPositionsController.GetAvailablePosition()
                .OccupyPosition().position;
            _stateMachine.GetThisEnemy.GetThisNavAgent.agent.destination = _destination;
        }
    }

    public override void Update()
    {
        if ((_stateMachine.transform.position - _destination).magnitude < 1)
        {
            _stateMachine.ChangeState(WheelDroneStates.Attack);
        }
    }
}
    
    
    

