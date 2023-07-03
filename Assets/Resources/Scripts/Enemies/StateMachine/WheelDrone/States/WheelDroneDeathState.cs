using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelDroneDeathState : AbstractState<WheelDroneStates>
{
    public WheelDroneDeathState(WheelDroneStateMachine stateMachine, WheelDroneStates thisState) 
    : base(stateMachine,  thisState){}

    public override void OnEnter()
    {
        _stateMachine.GetThisEnemy.GetAbstractEnemy.KillThisEnemy();
    }
}
