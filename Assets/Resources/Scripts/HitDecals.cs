using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDecals : MonoBehaviour, IPoolable
{
    [SerializeField] public float dissapearTimer = 5f;
    
    private ObjectPoolContainer _selfPool;
    private float _timer;

    private void OnEnable()
    {
        ResetItem();
    }

    public void GetFromPool()
    {
        gameObject.SetActive(true);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void ReleaseToPool()
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
            _selfPool.GetPool.Release(this);
        }
    }

    private void Update()
    {
        CheckTimer();
    }
}
