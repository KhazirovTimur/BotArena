using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable 
{
    public void SetParentPool(ObjectPoolContainer poolsContainer);

    public void OnGetFromPool();

    public void OnReleaseToPool();

    public GameObject GetGameObject();

    public void ResetItem();
  
}
