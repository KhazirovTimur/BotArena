using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
  
    private void Start()
    {
        GetComponentInParent<EnemySpawner>().AddSpawnPoint(transform);
    }
    
}
