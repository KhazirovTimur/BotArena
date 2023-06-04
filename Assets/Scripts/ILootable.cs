using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILootable
{
    public void SendToCollector(Transform targetPosition, ICollector inventory);
}
