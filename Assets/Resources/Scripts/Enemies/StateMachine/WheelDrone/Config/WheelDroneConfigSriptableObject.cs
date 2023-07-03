using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WheelDrone config", menuName = "State Machine Config / Wheel Drone")]
public class WheelDroneConfigSriptableObject : ScriptableObject
{
    [Header("Spawn state")] [SerializeField]
    private float chance;
}
