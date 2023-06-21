using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelDroneAttackState : AbstractState<WheelDroneStates>, IAttacker
{

    private bool havePermissionToAttack = false;
    private bool requestWasSent = false;

    private float cantAttackTimer = 2;
    private float _timer;

    public WheelDroneAttackState(WheelDroneStateMachine stateMachine, WheelDroneStates thisState) 
        : base(stateMachine,  thisState){}

    public override void OnEnter()
    {
        _timer = 0;
    }

    public override void Update()
    {
        CheckRequest();
        if (CanAttack() && havePermissionToAttack)
        {
            _stateMachine.GetThisEnemy.GetAttackPlayer.Shoot();
            _stateMachine.GetThisEnemy.GetAgressionController.ReleaseFromAttackers(this);
            havePermissionToAttack = false;
        }
        
        if(CanAttack())
            _timer = Time.time + cantAttackTimer;

        if (_timer < Time.time)
        {
            _stateMachine.ChangeState(WheelDroneStates.Move);
        }
    }

    public override void OnExit()
    {
        _stateMachine.GetThisEnemy.GetAgressionController.ReleaseFromAttackers(this);
        _stateMachine.GetThisEnemy.GetAgressionController.RemoveFromAttackersQueue(this);
        havePermissionToAttack = false;
        requestWasSent = false;
    }

    private void CheckRequest()
    {
        if (!requestWasSent && CanAttack())
        {
            _stateMachine.GetThisEnemy.GetAgressionController.TryGetPermissionToAttack(this);
            requestWasSent = true;
        }
        if (requestWasSent && !CanAttack())
        {
            _stateMachine.GetThisEnemy.GetAgressionController.RemoveFromAttackersQueue(this);
            requestWasSent = false;
        }
    }

    private bool CanAttack()
    {
        return _stateMachine.GetThisEnemy.GetAttackPlayer.CanShoot();
    }

    public void GivePermission()
    {
        havePermissionToAttack = true;
    }

    public float GetAttackPriority()
    {
        return _stateMachine.GetThisEnemy.GetAgressionController.CountAttackPriority(0, 0);
    }

}
