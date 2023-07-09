using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerPosPredictor : MonoBehaviour
{
    [SerializeField] private float delayUpdateLastPlayerPos = 0f;
    [SerializeField] private float playerHeight = 0.3f;
    
    private Transform _playerTransform;
    private Vector3 _lastPlayerPosition;
    private float _distanceToPlayer;
    private float _projectileTravelTime;
    private float _timerUpdateLastPlayerPos;

    private void Start()
    {
        _playerTransform = GetComponent<EnemyInterface>().GetPlayerTransform;
    }


    private void Update()
    {
        UpdatePlayerPosition();
        _timerUpdateLastPlayerPos = Time.time + delayUpdateLastPlayerPos;
        _distanceToPlayer = (transform.position - _playerTransform.position).magnitude;
    }
    
    public Vector3 PredictPlayerPosition(float projectileSpeed)
    {
        if (projectileSpeed == 0)
            return _playerTransform.position;
        _projectileTravelTime = _distanceToPlayer / projectileSpeed;
        return _playerTransform.position + (Vector3.up * playerHeight) + (GetPlayerDirection().normalized * GetPlayerSpeed() * _projectileTravelTime);
    }

    private void UpdatePlayerPosition()
    { 
        _lastPlayerPosition = _playerTransform.position;
    }

    private Vector3 GetPlayerDirection()
    {
        return _playerTransform.position - _lastPlayerPosition;
    }

    private float GetPlayerSpeed()
    {
        return GetPlayerDirection().magnitude / delayUpdateLastPlayerPos;
    }
    
}
