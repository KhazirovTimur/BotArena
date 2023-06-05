using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField]
    private float changeSpeed;
    

    [SerializeField]
    private Transform weaponSwitchPoint;
    private Vector3 switchPosition;
    private Quaternion switchRotation;

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    private bool needToChangeWeapon;
    private bool loweringWeapon;

    public Action WeaponWasLowered;
    public Action WeaponIsReady;



    private void Start()
    {
        defaultPosition = transform.localPosition;
        defaultRotation = transform.localRotation;
        switchPosition = weaponSwitchPoint.localPosition;
        switchRotation = weaponSwitchPoint.localRotation;
        needToChangeWeapon = false;
        loweringWeapon = true;
    }


    private void FixedUpdate()
    {
        if(needToChangeWeapon)
        {
            if(loweringWeapon)
                LowerWeapon();
            else
                ReadyWeapon();
        
        }
    }

    private void LowerWeapon()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, switchPosition, changeSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, switchRotation, changeSpeed * Time.deltaTime);
        if (transform.localPosition == switchPosition && transform.localRotation == switchRotation)
        {
            WeaponWasLowered();
            loweringWeapon = false;
        }
    }

    private void ReadyWeapon()
    {
        transform.localPosition = Vector3.Slerp(transform.localPosition, defaultPosition, changeSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, defaultRotation, changeSpeed * Time.deltaTime);
        if (transform.localPosition == defaultPosition && transform.localRotation == defaultRotation)
        {
            WeaponIsReady();
            needToChangeWeapon = false;
            loweringWeapon = true;
        }
    }

    public void SetTrueNeedToChangeWeapon()
    {
        needToChangeWeapon = true;
        loweringWeapon = true;
    }

}
