using System;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;



public class AttackComponent : MonoBehaviour, IAttacker
{
    private List<IEnemyAttack> rangeAttacks = new List<IEnemyAttack>();
    //private List<IEnemyAttack> meleeAttacks;

    private IEnemyAttack[] _attacks;
    
    private IEnemyAttack _currentAttack;

    private EnemyInterface _thisEnemy;
    private AbstractAnimation _thisAnim;

    private bool _attackOnReady;
    
    private bool _havePermissionToAttack;
    private bool _requestWasSent;

    private bool _canAttack;

    public Action CanAttack;
    public Action CanNotAttack;

    private bool _previouslyCanReachPlayerWithAttack;
    
    private float _cooldownTimer;

    private bool _inAttack;

    public static Transform PlayerTransform;

    private bool _initialized;

    

    public void InitializeAttacks()
    {
        _thisEnemy = GetComponent<EnemyInterface>();
        _thisAnim = _thisEnemy.GetAnimation;
        if(!PlayerTransform)
            PlayerTransform = FindObjectOfType<FirstPersonController>().transform;
        _attacks = GetComponentsInChildren<IEnemyAttack>();
        foreach (var attack in _attacks)
        {
            rangeAttacks.Add(attack);
        }
        _currentAttack = rangeAttacks[0];
        _currentAttack.FinishAttack += AtAttackFinish;
        _initialized = true;
    }


    private void Update()
    {
        if(!_initialized)
            InitializeAttacks();
        if(!_attackOnReady)
            return;
        CheckRequest();
        _thisAnim.AimAtPoint(_currentAttack.GetAimingPoint());
        if(_havePermissionToAttack)
            PerformAttack();
    }

    public static Transform GetPlayerTransform()
    {
        return PlayerTransform;
    }

    private bool OnCooldown()
    {
        return _cooldownTimer > Time.time;
    }


    private void PerformAttack()
    {
        _inAttack = true;
        _currentAttack.DoAttack();
        _havePermissionToAttack = false;
    }

    private void AtAttackFinish()
    {
        _cooldownTimer = Time.time + _currentAttack.GetCooldownTime();
        _thisEnemy.GetAgressionController.ReleaseFromAttackers(this);
        _inAttack = false;
    }


    private void CheckRequest()
    {
        if (!_requestWasSent && CheckCanAttack())
        {
            _thisEnemy.GetAgressionController.TryGetPermissionToAttack(this);
            _requestWasSent = true;
        }
        if (_requestWasSent && !CheckCanAttack())
        {
            _thisEnemy.GetAgressionController.RemoveFromAttackersQueue(this);
            _requestWasSent = false;
        }
    }
    
    private bool CheckCanAttack()
    {
        if (PlayerIsReachableForAttack() && !OnCooldown() && _thisAnim.IsAimedAtPoint(_currentAttack.GetAimingPoint()) && !_inAttack)
        {
            return true;
        }
        return false;
    }

    private bool PlayerIsReachableForAttack()
    {
        if (_currentAttack.ConditionsMet() && 
            _currentAttack.GetMaxAttackDistance() > (transform.position - _thisEnemy.GetPlayerTransform.position).magnitude)
        {
            if (!_previouslyCanReachPlayerWithAttack)
            {
                CanAttack?.Invoke();
                _previouslyCanReachPlayerWithAttack = true;
            }
            return true;
        }
        if (_previouslyCanReachPlayerWithAttack)
        {
            CanNotAttack?.Invoke();
            _previouslyCanReachPlayerWithAttack = false;
        }
        return false;
    }
    

    public void GivePermission()
    {
        _havePermissionToAttack = true;
    }

    public float GetAttackPriority()
    {
        return _thisEnemy.GetAgressionController.CountAttackPriority(0, 0);
    }

    public void StartAttackOnReady()
    {
        _attackOnReady = true;
        if(!PlayerIsReachableForAttack())
            CanNotAttack?.Invoke();
    }

    public void StopAllAttacks()
    {
        _attackOnReady = false;
        _thisAnim.ReturnToDefault();
        _thisEnemy.GetAgressionController.ReleaseFromAttackers(this);
        _thisEnemy.GetAgressionController.RemoveFromAttackersQueue(this);
    }



    public void PerformAttackWithoutPermission()
    {
        
    }

    public void ChangeCurrentAttack()
    {
        
    }

}
