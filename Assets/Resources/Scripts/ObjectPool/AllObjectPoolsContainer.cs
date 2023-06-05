using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Object = System.Object;


//Contains pools of different objects

public class AllObjectPoolsContainer : MonoBehaviour
{

    //public Dictionary<string, ObjectPool<IPoolable>> Pools = new Dictionary<string, ObjectPool<IPoolable>>();
    private List<ObjectPoolContainer> Pools = new List<ObjectPoolContainer>();


    public ObjectPoolContainer CreateNewPool(IPoolable reference, int defaultCapacity)
    {
        GameObject NewContainer = new GameObject();
        NewContainer.transform.SetParent(this.transform);
        ObjectPoolContainer NewPool = NewContainer.AddComponent<ObjectPoolContainer>();
        NewPool.CreatePool(reference, defaultCapacity);
        Pools.Add(NewPool);
        return NewPool;
    }
    
}
