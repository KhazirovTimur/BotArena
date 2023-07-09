using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum WheelDroneStates
{
    Spawn,
    MoveToShootingPoint,
    Chase,
    Death
}

public class WheelDroneStateMachine : AbstractStateMachine<WheelDroneStates>
{
    
    protected override void CreateStates()
    {
        WheelDroneSpawnState newSpawnState = new WheelDroneSpawnState(this, WheelDroneStates.Spawn);
        AddToDictionary(newSpawnState);
        WheelDroneAttackFromStaticPosState newAttackFromStaticPosState = new WheelDroneAttackFromStaticPosState(this, WheelDroneStates.MoveToShootingPoint);
        AddToDictionary(newAttackFromStaticPosState);
        WheelDroneChaseState newChaseState = new WheelDroneChaseState(this, WheelDroneStates.Chase);
        AddToDictionary(newChaseState);
        WheelDroneDeathState newDeathState = new WheelDroneDeathState(this, WheelDroneStates.Death);
        AddToDictionary(newDeathState);
        _currentState = newSpawnState;
        
    }

    public override void ChangeToDeathState()
    {
        ChangeState(WheelDroneStates.Death);
    }


}
