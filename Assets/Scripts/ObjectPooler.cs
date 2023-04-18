using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;


//Contains pools of different objects

public class ObjectPooler : MonoBehaviour
{
    
    private List<ObjectPool<GameObject>> Pools = new List<ObjectPool<GameObject>>();
    

    public int CreatePool(GameObject reference, int defaultCapacity)
    {
        Pools.Add( new ObjectPool<GameObject>(
            () =>
            {
                GameObject obj = Instantiate(reference);
                obj.transform.parent = this.transform;
                return obj;
            }, 
            gameobj => { gameobj.SetActive(true); }, 
            gameobj => { gameobj.SetActive(false); }, 
            gameobj => { Destroy(gameobj); }, 
            true, defaultCapacity, 500
            ));
        return Pools.Count - 1;
    }

    public ObjectPool<GameObject> GetPool(int index)
    {
        return Pools[index];
    }
}
