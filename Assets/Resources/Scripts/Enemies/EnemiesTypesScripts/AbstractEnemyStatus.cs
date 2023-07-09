using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;




public abstract class AbstractEnemyStatus : MonoBehaviour, IDamagable
{
    [Header("Basic stats")]
    [SerializeField] private float hp;
    [SerializeField] private int valueInMoney;
    [SerializeField] private GameObject destroyVFX;

    private StaticPosition _currentPosition;
    public StaticPosition GetCurrentStaticPosition => _currentPosition;
    
    private int oneCellValue;

    private int cellsCount;

    private ObjectPoolContainer moneyPool;

    private bool _isDead;

    public Action EnemyKilled;
    
    
    private StaticPositionsController _positionsController;
    public StaticPositionsController GetPositionsController => _positionsController;

    
    public void TakeDamage(float damage)
    {
        hp -= damage;
        CheckHP();
    }
    
    private void CheckHP()
    {
        if (hp <= 0 && !_isDead)
        {
            GetComponent<EnemyInterface>().GetToDeathStateMachine.ChangeToDeathState();
            _isDead = true;
        }      
    }

    public void KillThisEnemy()
    {
        ThrowMoney();
        ReleaseChildrenObjectsToPool();
        Instantiate(destroyVFX, transform.position + Vector3.up, transform.rotation);
        EnemyKilled?.Invoke();
        Destroy(this.gameObject);
    }


    private void ThrowMoney()
    {
        for (int i = 0; i < cellsCount; i++)
        {
            LootableItem item = moneyPool.GetPool.Get().GetGameObject().GetComponent<LootableItem>();
            item.GetGameobject.transform.position = transform.position;
            item.RandomThrowOnSpawn();
        }
    }
    
    private void CountCells()
    {
        cellsCount = valueInMoney / oneCellValue;
    }

    private void ReleaseChildrenObjectsToPool()
    {
        while(transform.GetComponentInChildren<IHardReleasedToPool>() != null)
        {    
            transform.GetComponentInChildren<IHardReleasedToPool>().HardReleasedToPool();
        }
    }
    
    public AbstractEnemyStatus SetMoneyPool(ObjectPoolContainer pool)
    {
        moneyPool = pool;
        return this;
    }
    
    public AbstractEnemyStatus SetOneCellValue(int value)
    {
        oneCellValue = value;
        CountCells();
        return this;
    }

    public AbstractEnemyStatus SetPositionController(StaticPositionsController controller)
    {
        _positionsController = controller;
        return this;
    }
    
    public AbstractEnemyStatus SetHpMultiplier(float multiplier)
    {
        hp *= multiplier;
        return this;
    }

    public void SetCurrentStaticPosition(StaticPosition pos)
    {
        _currentPosition = pos;
    }


}
