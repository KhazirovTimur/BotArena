using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDecals : MonoBehaviour, IPoolable, IHardReleasedToPool, IHitEffect
{
    [SerializeField] public float dissapearTimer = 5f;
    
    private ObjectPoolContainer _selfPool;
    private float _timer;

    private void OnEnable()
    {
        ResetItem();
    }

    public void OnGetFromPool()
    {
        gameObject.SetActive(true);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void OnReleaseToPool()
    {
        transform.SetParent(_selfPool.GetComponent<Transform>());
        gameObject.SetActive(false);
    }

    public void ResetItem()
    {
        _timer = Time.time + dissapearTimer;
    }

    public void SetParentPool(ObjectPoolContainer pool)
    {
        _selfPool = pool;
    }


    private void CheckTimer()
    {
        if (_timer < Time.time)
        {
            DestroyDecal();
        }
    }

    private void DestroyDecal()
    {
        if(!_selfPool)
        {
            Destroy(gameObject);
            return;
        }
        _selfPool.GetPool.Release(this);
    }

    public void HardReleasedToPool()
    {
        if(_selfPool) 
            transform.SetParent(_selfPool.GetComponent<Transform>());
        DestroyDecal();
    }

    public void SetPosAndRotation(RaycastHit hit)
    {
        transform.position = hit.point;
        transform.LookAt(transform.position + (-hit.normal));
        transform.SetParent(hit.transform);
    }

    private void Update()
    {
        CheckTimer();
    }
}
