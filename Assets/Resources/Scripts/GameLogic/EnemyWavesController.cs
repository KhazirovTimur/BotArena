using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWavesController : MonoBehaviour
{
    
    [SerializeField] private int startAmountOfEnemies = 10;
    [SerializeField] private int amountOfEnemiesIncreaseStep = 5;
    [SerializeField] private float startEnemiesHpMultiplier = 1;
    [SerializeField] private float hpMultiplierIncreaseStep = 0.1f;
    [SerializeField] private int startMaxEnemiesAlive = 5;
    [SerializeField] private int maxEnemiesAliveStep = 1;
    [SerializeField] private int startMAxAttackEnemies = 1;
    [SerializeField] private float maxAttackingEnemiesIncreaseStep = 0.2f;
    [SerializeField] private float restBetweenWaves = 15;
    public float GetRestTimer => restBetweenWaves;

    private int _survivedWaves = 0;
    public int GetSurvivedWaves => _survivedWaves;
    
    
    private int _amountOfEnemies;
    private float _enemiesHpMultiplier;
    private int _maxEnemiesAlive;
    private float _maxAttackingEnemies;
    private Dictionary<AllLocations.Locations, EnemySpawner> _spawners = new Dictionary<AllLocations.Locations, EnemySpawner>();
    private EnemiesAgressionController _agressionController;
    private bool _waveInProgress;
    private PlayerInterface _player;
    private float _waveStartTimer;

    public Action WaveWasEnded;

    private void Start()
    {
        _amountOfEnemies = startAmountOfEnemies;
        _enemiesHpMultiplier = startEnemiesHpMultiplier;
        _maxEnemiesAlive = startMaxEnemiesAlive;
        _maxAttackingEnemies = startMAxAttackEnemies;
        _player = FindObjectOfType<PlayerInterface>();
        _agressionController = FindObjectOfType<EnemiesAgressionController>();
        EnemySpawner[] allSpawners = FindObjectsOfType<EnemySpawner>();
        foreach (var spawner in allSpawners)
        {
            _spawners.Add(spawner.GetSpawnerLocation, spawner);
        }

        _waveStartTimer = Time.time + restBetweenWaves;
        WaveWasEnded?.Invoke();
    }

    private void Update()
    {
        if(_waveInProgress)
            return;
        if (!_waveInProgress && _waveStartTimer < Time.time) 
            Teleport.DisableTeleports();
        if (!_waveInProgress && _waveStartTimer < Time.time - 5) 
            CreateWave();
    }


    private void CreateWave()
    {
        _waveInProgress = true;
        _spawners[_player.GetPlayerStatus.GetCurrentLocation].SetAmountOfEnemiesToSpawn(_amountOfEnemies).
            SetMaxEnemiesAlive(_maxEnemiesAlive).SetEnemyHpMultiplier(_enemiesHpMultiplier); 
        _spawners[_player.GetPlayerStatus.GetCurrentLocation].AllEnemiesWereKilled += EndWave;
        _agressionController.SetMaxAttackers((int)_maxAttackingEnemies);
    }

    private void EndWave()
    {
        _waveInProgress = false;
        _survivedWaves += 1;
        _waveStartTimer = Time.time + restBetweenWaves;
        Teleport.EnableTeleports();
        IncreaseDifficulty();
        WaveWasEnded?.Invoke();
    }



    private void IncreaseDifficulty()
    {
        _amountOfEnemies += amountOfEnemiesIncreaseStep;
        _enemiesHpMultiplier += hpMultiplierIncreaseStep;
        _maxEnemiesAlive += maxEnemiesAliveStep;
        _maxAttackingEnemies += maxAttackingEnemiesIncreaseStep;
    }


}
