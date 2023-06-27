using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Grenade : MonoBehaviour, IProjectile
{
    [SerializeField] private float gravity = 15f;
    
    private float _bulletSpeed;
    private float _verticalSpeed;
    private LayerMask occlusionLayers;
    
    private AbstractShootMechanic parentShooter;
    

    private Rigidbody thisRb;

    private void Start()
    {
        thisRb = GetComponent<Rigidbody>();
        _verticalSpeed = Vector3.ProjectOnPlane(transform.forward * _bulletSpeed, transform.forward).magnitude;
    }

    private void FixedUpdate()
    {
        MoveProjectile();
        CheckObjectsAhead();
        AddGravity();
    }

    private void AddGravity()
    {
        _verticalSpeed -= gravity * Time.deltaTime;
    }

    private void MoveProjectile()
    {
        transform.Translate(transform.forward * _bulletSpeed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.up * _verticalSpeed * Time.deltaTime, Space.World);
    }

    
    //Function prevents "tunneling" fast projectiles through objects
    private void CheckObjectsAhead()
    {
        Vector3 dirrection =  (transform.position + (Vector3.up * (_verticalSpeed - gravity * Time.deltaTime))) 
                              + ((transform.forward * _bulletSpeed) - transform.position);
        if (Physics.Raycast(transform.position, dirrection,
                out RaycastHit hit, (dirrection - transform.position).magnitude * 0.08f, occlusionLayers))
        {
            MakeExplosion(hit);
            Destroy(gameObject);
        }
    }

    private void MakeExplosion(RaycastHit hit)
    {
        GameObject newExplosion = parentShooter.GetHitEffect();
        newExplosion.GetComponent<IHitEffect>().SetPosAndRotation(hit);
    }

    public void SetDamage(float damage)
    {
        
    }

    public void SetSpeed(float speed)
    {
        _bulletSpeed = speed;
    }

    public void ResetItem()
    {
        
    }

    public void SetOcclusionLayers(LayerMask mask)
    {
        occlusionLayers = mask;
    }

    public void SetParentShooter(AbstractShootMechanic parent)
    {
        parentShooter = parent;
    }
}
