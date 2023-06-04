using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public float delay = 2.5f;
    private float timer = 0;

    public GameObject energyCellPrefab;
    
    private ObjectPoolContainer projectilesPool;
    private ObjectPoolContainer EnergyCellsPool;

    private int oneCellValue;
    


    private void Start()
    {
        AttackPlayer component = enemyToSpawn.GetComponent<AttackPlayer>();
        projectilesPool = FindObjectOfType<AllObjectPoolsContainer>().
            CreateNewPool(component.projectile.GetComponent<IPoolable>(), 150);
        EnergyCellsPool = FindObjectOfType<AllObjectPoolsContainer>().
            CreateNewPool(energyCellPrefab.GetComponent<IPoolable>(), 150);
        oneCellValue = EnergyCellsPool.GetPool.Get().GetGameObject().GetComponent<LootableItem>().GetValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < Time.time)
        {
            SpawnEnemy();
            UpdateTimer();
        }

    }

    private void SpawnEnemy()
    {
        GameObject NewEnemy = Instantiate(enemyToSpawn, transform);
        NewEnemy.GetComponent<AttackPlayer>().SetObjectPoolContainer(projectilesPool);
        NewEnemy.GetComponent<SimpleEnemy>().SetMoneyPool(EnergyCellsPool).SetOneCellValue(oneCellValue);
    }

    private void UpdateTimer()
    {
        timer = Time.time + delay;
    }
}
