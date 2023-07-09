using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private StaticPositionsController positionsController;
    [SerializeField] private List<GameObject> enemiesToSpawn;
    [SerializeField] private float minSpawnDelay = 2.5f;
    [SerializeField] private AllLocations.Locations spawnerLocation;
    public AllLocations.Locations GetSpawnerLocation => spawnerLocation;
    private List<Transform> _spawnPoints = new List<Transform>();

    private float _spawnTimer;
    
    private int _amountOfEnemiesToSpawn; 
    private int _maxEnemiesAlive;
    private float _enemyHpMultiplier;
    
    
    public GameObject energyCellPrefab;
    private ObjectPoolContainer _energyCellsPool;
    private int _oneCellValue;

    public Action AllEnemiesWereKilled;



    private void Start()
    {
        _energyCellsPool = FindObjectOfType<AllObjectPoolsContainer>().
            GetPool(energyCellPrefab.GetComponent<IPoolable>());
        _oneCellValue = _energyCellsPool.GetPool.Get().GetGameObject().GetComponent<LootableItem>().GetValue;
    }
    


    // Update is called once per frame
    void Update()
    {
        if(CanSpawnEnemy())
            SpawnEnemy();
    }

    private bool CanSpawnEnemy()
    {
        if (_spawnTimer > Time.time)
            return false;
        if (transform.childCount - _spawnPoints.Count >= _maxEnemiesAlive)
            return false;
        if (_amountOfEnemiesToSpawn <= 0)
            return false;
        return true;
    }

    private void CheckAliveEnemies()
    {
        if(transform.childCount - _spawnPoints.Count == 1 && _amountOfEnemiesToSpawn <= 0)
            AllEnemiesWereKilled?.Invoke();
    }

    
    private void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)], 
            _spawnPoints[Random.Range(0, _spawnPoints.Count)]);
        newEnemy.transform.parent = transform;
        newEnemy.GetComponent<AbstractEnemyStatus>().SetMoneyPool(_energyCellsPool).SetOneCellValue(_oneCellValue)
            .SetPositionController(positionsController).SetHpMultiplier(_enemyHpMultiplier).EnemyKilled += CheckAliveEnemies;
        newEnemy.GetComponent<AttackComponent>().InitializeAttacks();
        _spawnTimer = Time.time + minSpawnDelay;
        _amountOfEnemiesToSpawn -= 1;
    }

    public void AddSpawnPoint(Transform point)
    {
        _spawnPoints.Add(point);
    }

    public EnemySpawner SetAmountOfEnemiesToSpawn(int amount)
    {
        _amountOfEnemiesToSpawn = amount;
        return this;
    }

    public EnemySpawner SetMaxEnemiesAlive(int amount)
    {
        _maxEnemiesAlive = amount;
        return this;
    }

    public EnemySpawner SetEnemyHpMultiplier(float multiplier)
    {
        _enemyHpMultiplier = multiplier;
        return this;
    }



}
