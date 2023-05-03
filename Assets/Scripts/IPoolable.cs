using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable 
{
    public void SetParentPool(ObjectPooler pooler, int index);

    public void GetFromPool();

    public void ReleaseToPool();

    public GameObject GetGameObject();

    public void ResetLifeTime();
}