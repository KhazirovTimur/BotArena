using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.Mathematics;
using UnityEngine;

public class WheelDroneAnimation : AbstractAnimation
{
    
    [Space(10)] [Header("Wheel drone special stats")] 
    [SerializeField] private float rotationsSpeed = 120;

    [SerializeField] private GameObject body;
    [SerializeField] private GameObject gun;
    
    private Vector3 _toTarget;
    private Quaternion _destination;
    private Quaternion _bodyDestination;
    private Quaternion _gunDestination;
    

    protected override void ReturnToDefaultPosition()
    {
        body.transform.rotation = Quaternion.RotateTowards(body.transform.localRotation, Quaternion.Euler(0,0,0), Time.deltaTime * rotationsSpeed);
        gun.transform.localRotation = Quaternion.RotateTowards(gun.transform.localRotation, Quaternion.Euler(0,0,0), Time.deltaTime * rotationsSpeed);
        if (body.transform.rotation == quaternion.Euler(0, 0, 0) &&
            gun.transform.rotation == quaternion.Euler(0, 0, 0))
            _inDefaultPosition = true;
    }

    public override void AimAtPoint(Vector3 point)
    {
        _toTarget = point  - body.transform.position;
        Vector3 bodyDestination = new Vector3(_toTarget.x, 0, _toTarget.z);
        Quaternion _destination = Quaternion.FromToRotation(Vector3.forward, bodyDestination);
        body.transform.rotation = Quaternion.Lerp(body.transform.rotation, _destination, Time.deltaTime * rotationsSpeed);

        _toTarget = point - gun.transform.position;
        Vector3 gunDestination = new Vector3(0, _toTarget.y, bodyDestination.magnitude);
        _destination = Quaternion.FromToRotation(Vector3.forward, gunDestination);
        gun.transform.localRotation = Quaternion.Lerp(gun.transform.localRotation, _destination, Time.deltaTime * rotationsSpeed);
    }


    public override bool IsAimedAtPoint(Vector3 point)
    {
        _toTarget = point - body.transform.position;
        if (Vector3.Angle(gun.transform.forward, _toTarget) < 25)
            return true;
        return false;
    }
}
