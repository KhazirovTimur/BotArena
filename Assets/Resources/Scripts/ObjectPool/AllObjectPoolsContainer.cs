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

    
    private Dictionary<IPoolable, ObjectPoolContainer> Pools = new Dictionary<IPoolable, ObjectPoolContainer>();


    public ObjectPoolContainer CreateNewPool(IPoolable reference, int defaultCapacity)
    {
        if (reference == null)
        {
            Debug.LogErrorFormat("Can't create pool, reference is null");
            return null;
        }
        GameObject NewContainer = new GameObject();
        NewContainer.transform.SetParent(this.transform);
        ObjectPoolContainer NewPool = NewContainer.AddComponent<ObjectPoolContainer>();
        NewPool.CreatePool(reference, defaultCapacity);
        Pools.Add(reference, NewPool);
        return NewPool;
    }

    public ObjectPoolContainer GetPool(IPoolable reference)
    {
        if (Pools.ContainsKey(reference))
            return Pools[reference];
        CreateNewPool(reference, 50);
        return Pools[reference];
    }
    
    
}
