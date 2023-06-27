using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitEffect
{
    public void SetPosAndRotation(RaycastHit hit);
}
