using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum WheelDroneStates
{
    Move,
    Attack,
    Death
}

public class WheelDroneStateMachine : AbstractStateMachine<WheelDroneStates>
{
    
    protected override void CreateStates()
    {
        WheelDroneMoveState newMoveState = new WheelDroneMoveState(this, WheelDroneStates.Move);
        AddToDictionary(newMoveState);
        WheelDroneAttackState newAttackState = new WheelDroneAttackState(this, WheelDroneStates.Attack);
        AddToDictionary(newAttackState);
        WheelDroneDeathState newDeathState = new WheelDroneDeathState(this, WheelDroneStates.Death);
        AddToDictionary(newDeathState);
        _currentState = newMoveState;
    }

    public override void ChangeToDeathState()
    {
        ChangeState(WheelDroneStates.Death);
    }


}
