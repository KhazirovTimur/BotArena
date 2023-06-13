using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleLineRendererVFXContainer : MonoBehaviour, IMuzzleVFX
{
    [SerializeField] private Transform barrelEnd;
    [SerializeField] private CustomLineVFX vfx;
    private ObjectPoolContainer _linePool;
    private AbstractWeapon _parentWeapon;
    private Vector3 _hitPoint;
    private float _turnOffVFXTimer;
    private bool _wasSetUp = false;


    private void OnEnable()
    {
        if (!_wasSetUp)
        {
            InitialSetUp();
            _wasSetUp = true;
        }
    }

    private void InitialSetUp()
    {
        _parentWeapon = GetComponentInParent<AbstractWeapon>();
        _linePool = FindObjectOfType<AllObjectPoolsContainer>()
            .CreateNewPool(vfx, _parentWeapon.GetDefaultPoolCapacity());
        if (_parentWeapon.TryGetComponent(out RaycastShot shotMechanic))
        {
            shotMechanic.HitTarget += SetHitPoint;
        }
    }

    public void PlayVFX()
    {
        IPoolable newLine = _linePool.GetPool.Get();
        CustomLineVFX lineComponent = newLine.GetGameObject().GetComponent<CustomLineVFX>();
        lineComponent.SetHitPoint(_hitPoint).SetInitialPoint(barrelEnd.transform.position);
    }

    public Transform GetBarrelEnd()
    {
        return barrelEnd;
    }
    
    private void SetHitPoint(Vector3 point)
    {
        _hitPoint = point;
    }

}
