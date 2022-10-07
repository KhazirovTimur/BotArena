using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullet : MonoBehaviour, IProjectile
{
    [SerializeField] private float _bulletLifeTime = 2.0f;
    
    private float _bulletSpeed = 40;
    private float _damage;
    
    

    private float _endOfLife;
    
    // Start is called before the first frame update
    void Start()
    {
        _endOfLife = Time.time + _bulletLifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > _endOfLife)
            Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetSpeed(float speed)
    {
        _bulletSpeed = speed;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
