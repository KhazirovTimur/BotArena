using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOutDestruction : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2;

    private float _timer;
    
    private void Start()
    {
        _timer = Time.time + lifeTime;
    }
    
    private void Update()
    {
        if(_timer < Time.time)
            Destroy(gameObject);
    }
}
