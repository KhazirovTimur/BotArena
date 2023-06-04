using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShot : MonoBehaviour, IShootMechanic
{
    public LayerMask occlusionLayers;
    public void DoShot(Transform barrelEnd, float damage)
    {
        if (Physics.Raycast(transform.position, barrelEnd.forward,
                out RaycastHit target, 1000, occlusionLayers))
        {
            Debug.DrawLine(barrelEnd.position, barrelEnd.position + barrelEnd.forward * target.distance, Color.blue, 0.5f);
            if (target.transform.TryGetComponent<IDamagable>(out IDamagable hitTarget))
            {
                hitTarget.TakeDamage(damage);
            }
        }
        Debug.DrawLine(barrelEnd.position, barrelEnd.position + barrelEnd.forward * 1000, Color.blue, 0.5f);
    }
}
