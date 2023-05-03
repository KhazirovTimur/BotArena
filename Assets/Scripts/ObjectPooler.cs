using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Object = System.Object;


//Contains pools of different objects

public class ObjectPooler : MonoBehaviour
{

    public Dictionary<string, ObjectPool<IPoolable>> Pools = new Dictionary<string, ObjectPool<IPoolable>>();
    
    

    public void CreatePool(IPoolable reference, int defaultCapacity, string index)
    {
        if (Pools.ContainsKey(index))
        {
            Debug.LogError("Pool already have name" + index + " please, change it");
            return;
        }
        ObjectPool<IPoolable> ToAdd = new ObjectPool<IPoolable>(
            () =>
            {
                GameObject obj = Instantiate(reference.GetGameObject());
                obj.transform.parent = this.transform;
                IPoolable newObject = obj.GetComponent<IPoolable>();
                newObject.SetParentPool(this, index);
                return newObject;
            },
            gameobj =>
            {
                gameobj.GetFromPool(); 
                gameobj.ResetLifeTime();
            },
            gameobj => { gameobj.ReleaseToPool(); },
            gameobj => { Destroy(gameobj.GetGameObject()); },
            true, defaultCapacity, 500);
        Pools.Add(index, ToAdd);
    }

    public ObjectPool<IPoolable> GetPool(string index)
    {
        return Pools[index];
    }
}
