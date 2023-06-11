using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleLineRendererVFXContainer : MonoBehaviour, IMuzzleVFX
{
    [SerializeField] private float turnOffTimer;
    [SerializeField] private Transform barrelEnd;
    private LineRenderer _vfx;
    private AbstractWeapon _parentWeapon;
    private Vector3 _hitPoint;
    private float _turnOffVFXTimer;
    

    private void Start()
    {
        _vfx = GetComponentInChildren<LineRenderer>();
        _parentWeapon = GetComponentInParent<AbstractWeapon>();
        if (_parentWeapon.TryGetComponent(out RaycastShot shotMechanic))
        {
            shotMechanic.HitTarget += SetHitPoint;
        }
    }

    public void PlayVFX()
    {
        ResetTurnOffVFXTimer();
        _vfx.SetPosition(0, barrelEnd.position);
        _vfx.SetPosition(_vfx.positionCount - 1, _hitPoint);
        _vfx.gameObject.SetActive(true);
        _vfx.transform.parent = null;
        ResetHitPoint();
    }

    private void TurnOffVFX()
    {
        _vfx.transform.parent = transform;
        _vfx.gameObject.SetActive(false);
    }

    private void SetHitPoint(RaycastHit hit)
    {
        _hitPoint = hit.point;
    }

    private void ResetHitPoint()
    {
        _hitPoint = barrelEnd.position + barrelEnd.transform.forward * 500;
    }

    private void ResetTurnOffVFXTimer()
    {
        _turnOffVFXTimer = Time. time + turnOffTimer;
    }

    private bool timeIsUp()
    {
        if (_turnOffVFXTimer < Time.time)
            return true;
        return false;
    }

    private void Update()
    {
        if(timeIsUp())
            TurnOffVFX();
    }
}
