using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ILootable loot))
        {
            loot.SendToCollector(this.transform, this.gameObject.GetComponent<PlayerInventory>());
        }
    }
}
