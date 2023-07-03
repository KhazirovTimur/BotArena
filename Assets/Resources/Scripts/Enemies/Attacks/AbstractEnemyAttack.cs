using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyAttackType
{
    Range,
    Meele
}

public abstract class AbstractEnemyAttack : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float maxRange;
    [SerializeField] private EnemyAttackType attackType;

    private Action _canAttack;
    private Action _canNotAttack;
    private Action _startAttack;
    private Action _finishAttack;
    
    // Start is called before the first frame update
    void Start()
    {
        switch (attackType)
        {
            case EnemyAttackType.Range:
            {
                
                break;
            }
            case EnemyAttackType.Meele:
            {
                
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DoAttack(){}

    public float GetMaxAttackDistance()
    {
        return maxRange;
    }

    public Action GetCanAttackAction()
    {
        return _canAttack;
    }

    public Action GetCanNotAttackAction()
    {
        return _canNotAttack;
    }

    public Action GetStartAttackAction()
    {
        return _startAttack;
    }

    public Action GetFinishAttackAction()
    {
        return _finishAttack;
    }
    
}
