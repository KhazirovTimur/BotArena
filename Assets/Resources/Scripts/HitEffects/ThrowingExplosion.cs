using System;
using StarterAssets;
using UnityEngine;

public class ThrowingExplosion : MonoBehaviour
{
    [SerializeField] private float maxStartSpeed;
    [SerializeField] private float lifeTime = 2.0f;
    
    private float _maxRadius;

    private ParticleSystem _vfx;

    private float _lifeTimer;



    private void Start()
    {
        _lifeTimer = Time.time + lifeTime;
        _maxRadius = GetComponent<CapsuleCollider>().radius;
        _vfx = GetComponentInChildren<ParticleSystem>();
        _vfx.Play();
    }

    private void Update()
    {
        if(_lifeTimer < Time.time)
            Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            FirstPersonController player = other.transform.GetComponentInParent<FirstPersonController>();
            Vector3 dirrection = (other.transform.position + Vector3.up) - transform.position;
            float speed = (dirrection.magnitude / _maxRadius) * maxStartSpeed;
            dirrection = dirrection.normalized;
            player.LaunchInAir(dirrection, speed);
            Debug.Log("push");
        }
    }
    
    
    
    
    
}
