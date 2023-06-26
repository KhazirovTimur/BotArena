using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private float _bulletSpeed;
    [SerializeField] private LayerMask occlusionLayers;
    [SerializeField] private GameObject hitEffect;
    
    private RaycastHit hit;
    
    private void FixedUpdate()
    {
        MoveProjectile();
        CheckObjectsAhead();
    }
    
    private void MoveProjectile()
    {
        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);
    }

    
    //Function prevents "tunneling" fast projectiles through objects
    private void CheckObjectsAhead()
    {
        if (Physics.Raycast(transform.position, transform.forward,
                out hit, _bulletSpeed * 0.02f, occlusionLayers))
        {
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if (Physics.Raycast(transform.position, -transform.up,
                out hit, (-GetComponent<Rigidbody>().velocity.y) * 0.02f, occlusionLayers))
        {
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(this);
    }

    public void SetDamage(float damage)
    {
        
    }

    public void SetSpeed(float speed)
    {
        _bulletSpeed = speed;
    }
    
}
