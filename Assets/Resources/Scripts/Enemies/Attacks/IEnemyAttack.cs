using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttack
{
    public float GetMaxAttackDistance();

    public void DoAttack();

    public Action GetCanAttackAction();
    
    public Action GetCanNotAttackAction();
    
    public Action GetStartAttackAction();
    
    public Action GetFinishAttackAction();

}
