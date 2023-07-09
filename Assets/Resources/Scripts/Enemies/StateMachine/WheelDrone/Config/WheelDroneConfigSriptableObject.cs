using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WheelDrone config", menuName = "State Machine Config / Wheel Drone")]
public class WheelDroneConfigSriptableObject : ScriptableObject
{
    [Header("Spawn state")] 
    [Tooltip("Chance of choosing chase state instead of shooting from static point from 0 to 1")]
    [SerializeField] private float chaseStateChance;
}
