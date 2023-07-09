using System;
using StarterAssets;
using UnityEngine;

public class ThrowingExplosion : MonoBehaviour, IHitEffect
{
    [SerializeField] private float maxStartSpeed;
    [SerializeField] private float vfxLifeTime = 2.0f;
    [SerializeField] private float throwEffectLifeTime = 0.5f;
    [SerializeField] private AudioClip explosionSound;
    

    private float _maxRadius;

    private ParticleSystem _vfx;

    private float _vfxLifeTimer;
    private float _throwEffectTimer;



    private void Start()
    {
        _vfxLifeTimer = Time.time + vfxLifeTime;
        _throwEffectTimer = Time.time + throwEffectLifeTime;
        _maxRadius = GetComponent<CapsuleCollider>().radius;
        _vfx = GetComponentInChildren<ParticleSystem>();
        _vfx.Play();
        GetComponent<AudioSource>().PlayOneShot(explosionSound);
    }

    private void Update()
    {
        if(_vfxLifeTimer < Time.time)
            Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if(_throwEffectTimer < Time.time)
            return;
        if (other.gameObject.CompareTag("Player"))
        {
            FirstPersonController player = other.transform.GetComponentInParent<FirstPersonController>();
            Vector3 dirrection = (other.transform.position + Vector3.up) - transform.position;
            float speed = (dirrection.magnitude / _maxRadius) * maxStartSpeed;
            dirrection = dirrection.normalized;
            player.LaunchInAir(dirrection, speed);
        }
    }
    
    public void SetPosAndRotation(RaycastHit hit)
    {
        transform.position = hit.point;
        transform.LookAt(transform.position + (-hit.normal));
        transform.SetParent(hit.transform);
    }
    
    
}
