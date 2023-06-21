using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInterface : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    public PlayerInventory GetPlayerInventory => _playerInventory;
    
    public Transform GetPlayerTransform => transform;
}
