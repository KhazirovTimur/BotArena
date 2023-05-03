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

    public Dictionary<int, ObjectPool<IPoolable>> Pools = new Dictionary<int, ObjectPool<IPoolable>>();

    private int counter = 0;
    

    public int CreatePool(IPoolable reference, int defaultCapacity)
    {
        Debug.Log("counter before creating " + counter);
        ObjectPool<IPoolable> ToAdd = new ObjectPool<IPoolable>(
            () =>
            {
                GameObject obj = Instantiate(reference.GetGameObject());
                obj.transform.parent = this.transform;
                IPoolable newObject = obj.GetComponent<IPoolable>();
                newObject.SetParentPool(this, counter - 1);
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
        Pools.Add(counter, ToAdd);
        Debug.Log("counter before increase " + counter);
        counter += 1;
        Debug.Log("counter after increase " + counter);
        Debug.Log("counter - 1 " + (counter - 1));
        return (counter-1);
    }

    public ObjectPool<IPoolable> GetPool(int index)
    {
        return Pools[index];
    }
}
