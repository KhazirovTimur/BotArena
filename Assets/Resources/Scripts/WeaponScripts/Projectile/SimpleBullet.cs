using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class SimpleBullet : MonoBehaviour, IProjectile, IPoolable
{
    
    
    //after this time will be destroyed
    [SerializeField] private float bulletLifeTime = 2.0f;
    
    //main projectile settings, AbstractWeapon set this
    private float _bulletSpeed;
    private float _damage;
    
    private RaycastHit hit;
    
    //Time, when bullet must be destroyed
    private float _bulletLifeTimer;
    
    private LayerMask occlusionLayers;

    private AbstractShootMechanic _parentShooter;

    private Vector3 _initialPoint;
    
    //cache for object pool
    private ObjectPoolContainer _selfPoolContainer;

    


    private void OnEnable()
    {
        ResetItem();
    }

    //set timer
    void Start()
    {
        _bulletLifeTimer = Time.time + bulletLifeTime;
    }

    // Check time to destroy projectile
    void Update()
    {
        CheckDestroyTimer();
    }
    
    
    private void FixedUpdate()
    {
        MoveProjectile();
        CheckObjectsAhead();
    }


    private void CheckDestroyTimer()
    {
        if(Time.time > _bulletLifeTimer)
            _selfPoolContainer.GetPool.Release(this);
    }


    private void MoveProjectile()
    {
        transform.Translate(transform.forward * _bulletSpeed * Time.deltaTime, Space.World);
    }

    
    //Function prevents "tunneling" fast projectiles through objects
    private void CheckObjectsAhead()
    {
        if (Physics.Raycast(transform.position, transform.forward,
                out hit, _bulletSpeed * 0.02f, occlusionLayers))
        {
            GameObject bulletHole = _parentShooter.GetHitEffect();
            bulletHole.GetComponent<IHitEffect>().SetPosAndRotation(hit);
            if (hit.transform.TryGetComponent<IDamagable>(out IDamagable target))
            {
                target.TakeDamage(CountReducedDamage());
            }
            _selfPoolContainer.GetPool.Release(this);
        }
    }


    public void SetOcclusionLayers(LayerMask mask)
    {
        occlusionLayers = mask;
    }

    public void SetParentPool(ObjectPoolContainer poolsContainer)
    {
        _selfPoolContainer = poolsContainer;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void OnGetFromPool()
    {
        this.gameObject.SetActive(true);
    }
    
    public void OnReleaseToPool()
    {
        this.gameObject.SetActive(false);
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetSpeed(float speed)
    {
        _bulletSpeed = speed;
    }

    public void ResetItem()
    {
        _bulletLifeTimer = Time.time + bulletLifeTime;
        _initialPoint = transform.position;
    }
    
    public void SetParentShooter(AbstractShootMechanic parent)
    {
        _parentShooter = parent;
    }

    private float GetTravelDistance()
    {
        return (_initialPoint - transform.position).magnitude;
    }

    private float CountReducedDamage()
    {
        if (_parentShooter.TryGetComponent(out IProjectileShootingMechanic shootMech))
        {
            return shootMech.GetDamageReducedByDistanceProjectile(GetTravelDistance(), _damage);
        }

        return _damage;
    }

}
