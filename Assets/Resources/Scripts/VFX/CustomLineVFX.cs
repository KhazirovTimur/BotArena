using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLineVFX : MonoBehaviour, IPoolable
{
    [SerializeField] private float startEraseSpeed;
    [SerializeField] private float eraseSpeedIncreaseRate;

    private float _currentSpeed;
    private LineRenderer _vfx;
    private ObjectPoolContainer _selfPool;
    private bool _wasInitialized = false;

    private void OnEnable()
    {
        if(!_wasInitialized)
            Initialize();
    }

    public void Initialize()
    {
        _vfx = GetComponent<LineRenderer>();
        _wasInitialized = true;
        _currentSpeed = startEraseSpeed;
    }
    
    private void Update()
    {
        _vfx.SetPosition(0, MoveStartToEnd());
        IncreaseSpeed();
        if(StartAtEnd())
            _selfPool.GetPool.Release(this);
    }

    private Vector3 MoveStartToEnd()
    {
        return _vfx.GetPosition(0) +
               (_vfx.GetPosition(1) - _vfx.GetPosition(0)).normalized * _currentSpeed * Time.deltaTime;
    }

    private void IncreaseSpeed()
    {
        _currentSpeed += _currentSpeed * eraseSpeedIncreaseRate * Time.deltaTime;
    }

    public CustomLineVFX SetHitPoint(Vector3 point)
    {
        _vfx.SetPosition(1, point);
        return this;
    }

    public CustomLineVFX SetInitialPoint(Vector3 point)
    {
        _vfx.SetPosition(0, point);
        return this;
    }

    private bool StartAtEnd()
    {
        return (_vfx.GetPosition(0) - _vfx.GetPosition(1)).magnitude < (_currentSpeed / 60);
    }

    public void SetParentPool(ObjectPoolContainer poolsContainer)
    {
        _selfPool = poolsContainer;
    }

    public void OnGetFromPool()
    {
        gameObject.SetActive(true);
    }

    public void OnReleaseToPool()
    {
        ResetItem();
        gameObject.SetActive(false);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void ResetItem()
    {
        _currentSpeed = startEraseSpeed;
    }
}
