using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class LootableItem : MonoBehaviour, ILootable, IPoolable
{
    [SerializeField] private int value = 5;
    public int GetValue => value;

    [Tooltip("Acceleration while move to collector")]
    [SerializeField] private float acceleration;
    [Tooltip("How close wil get to collector, before being collected")]
    [SerializeField] private float minDistance = 1;
    [Tooltip("How fast will loose speed on initial throw")]
    [SerializeField] private float minDeceleration;
    [Tooltip("How fast will loose speed on initial throw")]
    [SerializeField] private float maxDeceleration;
    [Tooltip("Minimal speed on initial throw")]
    [SerializeField] private float minThrowSpeed;
    [Tooltip("Maximum speed on initial throw")]
    [SerializeField] private float maxThrowSpeed;
    private bool _thrownOnStart;
    private bool _isMoving;
    private Transform _destination;
    private Vector3 _throwDestination;
    private float _speed;
    private ICollector _targetInventory;
    private float _deceleration;
    private float _throwSpeed;

    public GameObject GetGameobject => gameObject;

    private ObjectPoolContainer _objectPool;

    private void Awake()
    {
        _isMoving = false;
        _thrownOnStart = false;
    }
    
    public void RandomThrowOnSpawn()
    {
        transform.rotation = Quaternion.Euler(Random.Range(170.0f, 370.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        _throwSpeed = Random.Range(minThrowSpeed, maxThrowSpeed);
        _deceleration = Random.Range(minDeceleration, maxDeceleration);
        _thrownOnStart = true;
    }


    private void ThrowMove()
    {
        _throwSpeed -= _deceleration;
        if (_throwSpeed < 0.01f)
        {
            FinishThrowMove();
            return;
        }
        transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward,
            _throwSpeed * Time.deltaTime);
    }

    private void FinishThrowMove()
    {
        _thrownOnStart = false;
    }

    public void SendToCollector(Transform targetTransform, ICollector inventory)
    {
        _destination = targetTransform;
        _speed = 0;
        _isMoving = true;
        _targetInventory = inventory;
    }
    
    private void MoveToTarget()
    {
        _speed += acceleration * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, _destination.position, _speed * Time.deltaTime);
    }

    private bool ReachMinDistanceToTarget()
    {
        float currentDistance = (transform.position - _destination.position).magnitude;
        return currentDistance < minDistance;
    }

    private void CollectThisItem()
    {
        _targetInventory.AddValue(value);
        _objectPool.GetPool.Release(this);
    }
    
    
    private void Update()
    {
        if (_thrownOnStart)
        {
            ThrowMove();
        }
        if (!_isMoving)
            return;
        MoveToTarget();
        if(ReachMinDistanceToTarget())
            CollectThisItem();
    }
    
    
    //Methods from IPoolable
    public void SetParentPool(ObjectPoolContainer poolsContainer)
    {
        _objectPool = poolsContainer;
    }

    public void GetFromPool()
    {
        gameObject.SetActive(true);
    }

    public void ReleaseToPool()
    {
        ResetItem();
        gameObject.SetActive(false);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void ResetItem()
    {
        _thrownOnStart = false;
        _isMoving = false;
        _speed = 0;
    }

    
}
