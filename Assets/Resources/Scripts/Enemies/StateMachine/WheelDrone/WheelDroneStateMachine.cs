using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum WheelDroneStates
{
    Spawn,
    Move,
    Attack,
    Death
}

public class WheelDroneStateMachine : AbstractStateMachine<WheelDroneStates>
{
    
    protected override void CreateStates()
    {
        WheelDroneSpawnState newSpawnState = new WheelDroneSpawnState(this, WheelDroneStates.Spawn);
        AddToDictionary(newSpawnState);
        WheelDroneMoveState newMoveState = new WheelDroneMoveState(this, WheelDroneStates.Move);
        AddToDictionary(newMoveState);
        WheelDroneAttackState newAttackState = new WheelDroneAttackState(this, WheelDroneStates.Attack);
        AddToDictionary(newAttackState);
        WheelDroneDeathState newDeathState = new WheelDroneDeathState(this, WheelDroneStates.Death);
        AddToDictionary(newDeathState);
        _currentState = newSpawnState;
    }

    public override void ChangeToDeathState()
    {
        ChangeState(WheelDroneStates.Death);
    }


}
