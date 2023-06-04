using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolContainer : MonoBehaviour
{
    private ObjectPool<IPoolable> pool;

    public ObjectPool<IPoolable> GetPool => pool;

    public ObjectPoolContainer CreatePool(IPoolable reference, int defaultCapacity)
    {
        pool = new ObjectPool<IPoolable>(
            () =>
            {
                GameObject obj = Instantiate(reference.GetGameObject());
                obj.transform.parent = this.transform;
                IPoolable newObject = obj.GetComponent<IPoolable>();
                newObject.SetParentPool(this);
                return newObject;
            },
            gameobj =>
            {
                gameobj.GetFromPool(); 
                gameobj.ResetItem();
            },
            gameobj => { gameobj.ReleaseToPool(); },
            gameobj => { Destroy(gameobj.GetGameObject()); },
            true, defaultCapacity, 500);
        return this;
    }
    

}
