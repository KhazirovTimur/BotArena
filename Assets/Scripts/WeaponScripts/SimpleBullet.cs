using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class SimpleBullet : MonoBehaviour, IProjectile
{
    
    //after this time will be destroyed
    [SerializeField]
    private float _bulletLifeTime = 2.0f;
    
    //main projectile settings, AbstractWeapon set this
    private float _bulletSpeed;
    private float _damage;
    
    private RaycastHit hit;
    
    //Time, when bullet must be destroyed
    private float _endOfLife;
    
    //cache for object pool
    private ObjectPooler _pooler;
    private int _poolIndex;

    //set timer
    void Start()
    {
        _endOfLife = Time.time + _bulletLifeTime;
    }

    // Check time to destroy projectile
    void Update()
    {
        if(Time.time > _endOfLife)
            _pooler.GetPool(_poolIndex).Release(this.gameObject);
    }
    
    
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
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                out hit, _bulletSpeed * 0.02f))
        {
            if (hit.transform.TryGetComponent<IDamagable>(out IDamagable target))
            {
                target.TakeDamage(_damage);
            }
            //Destroy(this.gameObject);
            _pooler.GetPool(_poolIndex).Release(this.gameObject);
        }
    }

    
    
    public void SetParentPool(ObjectPooler pooler, int index)
    {
        _pooler = pooler;
        _poolIndex = index;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetSpeed(float speed)
    {
        _bulletSpeed = speed;
    }

    public void ResetLifeTime()
    {
        _endOfLife = Time.time + _bulletLifeTime;
    }


}
