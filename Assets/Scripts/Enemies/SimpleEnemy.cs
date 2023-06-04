using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SimpleEnemy : MonoBehaviour, IDamagable
{
    [SerializeField]
    private float hp;
    [SerializeField]
    private int valueInMoney;
    private int oneCellValue;

    private int cellsCount;

    private ObjectPoolContainer moneyPool;
    

    public void TakeDamage(float damage)
    {
        Debug.Log("You hit me on " + damage + " damage");
        hp -= damage;
        CheckHP();
    }
    
    private void CheckHP()
    {
        if (hp <= 0)
            KillThisEnemy();        
    }

    private void KillThisEnemy()
    {
        Debug.Log("I'm dead(");
        ThrowMoney();
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

    public SimpleEnemy SetMoneyPool(ObjectPoolContainer pool)
    {
        moneyPool = pool;
        return this;
    }
    
    public SimpleEnemy SetOneCellValue(int value)
    {
        oneCellValue = value;
        CountCells();
        return this;
    }

    private void CountCells()
    {
        cellsCount = valueInMoney / oneCellValue;
    }

}
