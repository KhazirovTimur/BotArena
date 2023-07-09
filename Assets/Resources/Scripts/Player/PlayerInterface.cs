using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInterface : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    public PlayerInventory GetPlayerInventory => _playerInventory;
    public Transform GetPlayerTransform => transform;
    
    private PlayerStatus _playerStatus;
    public PlayerStatus GetPlayerStatus => _playerStatus;

    private void Awake()
    {
        _playerStatus = GetComponent<PlayerStatus>();
        _playerInventory = GetComponentInChildren<PlayerInventory>();
    }
}
