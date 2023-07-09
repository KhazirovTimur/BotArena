using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelDroneSpawnState : AbstractState<WheelDroneStates>
{

    private float _chaseStateChance = 0.5f;
    
    public WheelDroneSpawnState(AbstractStateMachine<WheelDroneStates> stateMachine, WheelDroneStates thisState) : base(stateMachine, thisState)
    {
    }

    public override void OnEnter()
    {
        _stateMachine.GetThisEnemy.GetThisNavAgent.MoveToPoint(
           new Vector3(Random.Range(1, 2), 0 , Random.Range(1, 2)));
        if(Random.Range(0.0f, 1.0f) < _chaseStateChance)
            _stateMachine.ChangeState(WheelDroneStates.Chase);
        else
        {
            _stateMachine.ChangeState(WheelDroneStates.MoveToShootingPoint);
        }
    }

    public override void Updater()
    {
      
    }
}
    
    
    

