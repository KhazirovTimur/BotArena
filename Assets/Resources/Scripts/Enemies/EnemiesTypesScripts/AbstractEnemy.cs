using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class AbstractEnemy : MonoBehaviour, IDamagable
{
    [Header("Basic stats")]
    [SerializeField]
    private float hp;
    [SerializeField]
    private int valueInMoney;
    
    private int oneCellValue;

    private int cellsCount;

    private ObjectPoolContainer moneyPool;
    

    private StaticPositionsController _positionsController;
    public StaticPositionsController GetPositionsController => _positionsController; 
    

    public void TakeDamage(float damage)
    {
        Debug.Log("You hit me on " + damage + " damage");
        hp -= damage;
        CheckHP();
    }
    
    private void CheckHP()
    {
        if (hp <= 0)
            GetComponent<EnemyInterface>().GetToDeathStateMachine.ChangeToDeathState();      
    }

    public void KillThisEnemy()
    {
        Debug.Log("I'm dead(");
        ThrowMoney();
        ReleaseChildrenObjectsToPool();
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

    public AbstractEnemy SetMoneyPool(ObjectPoolContainer pool)
    {
        moneyPool = pool;
        return this;
    }
    
    public AbstractEnemy SetOneCellValue(int value)
    {
        oneCellValue = value;
        CountCells();
        return this;
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

    public void SetPositionController(StaticPositionsController controller)
    {
        _positionsController = controller;
    }

}
