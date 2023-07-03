using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class WheelDrone : AbstractEnemy
{
    [Space(10)] [Header("Wheel drone special stats")] 
    [SerializeField] private float rotationsSpeed = 120;

    [SerializeField] private GameObject body;
    [SerializeField] private GameObject gun;

    private Transform _playerTransform;

    private bool _trackPlayer = true;

    private Vector3 _toPlayer;
    private Quaternion _destination;
    private Quaternion _bodyDestination;
    private Quaternion _gunDestination;
    


    private void Start()
    {
        _playerTransform = FindObjectOfType<FirstPersonController>().transform;
    }

    private void FixedUpdate()
    {
        if (_trackPlayer)
        {
            AimAtPlayer();
        }
        
        Debug.Log(IsRotatedToPlayer());
    }
    
    private void AimAtPlayer()
    {
        _toPlayer = (_playerTransform.position + (Vector3.left * 0.5f)) - body.transform.position;
        Vector3 bodyDestination = new Vector3(_toPlayer.x, 0, _toPlayer.z);
        Quaternion _destination = Quaternion.FromToRotation(Vector3.forward, bodyDestination);
        body.transform.rotation = Quaternion.RotateTowards(body.transform.rotation, _destination, Time.deltaTime * rotationsSpeed);

        _toPlayer = (_playerTransform.position + Vector3.up) - gun.transform.position;
        Vector3 gunDestination = new Vector3(0, _toPlayer.y, bodyDestination.magnitude);
        _destination = Quaternion.FromToRotation(Vector3.forward, gunDestination);
        gun.transform.localRotation = Quaternion.RotateTowards(gun.transform.localRotation, _destination, Time.deltaTime * rotationsSpeed);
    }


    private bool IsRotatedToPlayer()
    {
        if (Vector3.Angle(gun.transform.forward, _toPlayer) < 15)
            return true;
        return false;
    }

}
