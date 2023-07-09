using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttack
{
    delegate void EventHandler();
    public event EventHandler FinishAttack;
    
    public float GetMaxAttackDistance();

    public void DoAttack();

    public bool ConditionsMet();

    public float GetCooldownTime();

    public Vector3 GetAimingPoint();
    
}
