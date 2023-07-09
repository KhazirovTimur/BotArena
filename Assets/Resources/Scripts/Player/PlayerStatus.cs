using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IDamagable
{
    [SerializeField] private AllLocations.Locations startLocation = AllLocations.Locations.Entrance;
    [SerializeField] private float maxHP = 100;

    private float _currentHP;
    private bool _isDead;
    private AllLocations.Locations _currentLocation;
    public AllLocations.Locations GetCurrentLocation => _currentLocation;

    public Action UpdateHP;

    private void Awake()
    {
        _currentLocation = startLocation;
        _currentHP = maxHP;
    }

    private void Start()
    {
        UpdateHP?.Invoke();
    }

    private void CheckHP()
    {
        if (_currentHP <= 0 && !_isDead)
        {
            _isDead = true;
            Debug.LogError("You are dead, samurai");
        }
    }

    public void ResetHP()
    {
        _currentHP = maxHP;
        UpdateHP?.Invoke();
    }

    public void SetCurrentLocation(AllLocations.Locations location)
    {
        _currentLocation = location;
    }

    public float GetCurrentHp()
    {
        return _currentHP;
    }

    public void TakeDamage(float damage)
    {
        _currentHP -= damage;
        CheckHP();
        UpdateHP?.Invoke();
    }
}
