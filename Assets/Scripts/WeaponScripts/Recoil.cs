using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [Header("Reference Points")]
    private Transform recoilPosition;
    private Transform rotationPoint;
    [Space(10)]

    [Header("Speed Settings")]
    public float positionalRecoilSpeed = 8f;
    public float rotationalRecoilSpeed = 8f;
    [Space(10)]

    public float positionalReturnSpeed = 18f;
    public float rotationalReturnSpeed = 38f;
    [Space(10)]

    [Header("Amount Settings:")]
    public Vector3 RecoilRotation = new Vector3(10, 5, 7);
    public Vector3 RecoilKickBack = new Vector3(0.015f, 0f, -0.2f);
    [Space(10)]


    Vector3 rotationalRecoil;
    Vector3 positionalRecoil;
    Vector3 Rot;
    
    private AbstractWeapon _weapon;
    
    void Start()
    {
        if (this.TryGetComponent(out AbstractWeapon thisWeapon))
        {
            _weapon = thisWeapon;
        }

        recoilPosition = this.transform;
        rotationPoint = this.transform;

        _weapon.ShotWasMade += AddRecoil;
    }

    private void AddRecoil()
    {
        rotationalRecoil += new Vector3(-RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
        rotationalRecoil += new Vector3(Random.Range(-RecoilKickBack.x, RecoilKickBack.x), Random.Range(-RecoilKickBack.y, RecoilKickBack.y), RecoilKickBack.z);
    }


    private void FixedUpdate()
    {
        rotationalRecoil = Vector3.Lerp(rotationalRecoil, Vector3.zero, rotationalReturnSpeed * Time.deltaTime);
        positionalRecoil = Vector3.Lerp(positionalRecoil, Vector3.zero, positionalReturnSpeed * Time.deltaTime);

        recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, positionalRecoil, positionalRecoilSpeed * Time.deltaTime);
        Rot = Vector3.Slerp(Rot, rotationalRecoil, rotationalRecoilSpeed * Time.deltaTime);
        rotationPoint.localRotation = Quaternion.Euler(Rot);
    }

}
