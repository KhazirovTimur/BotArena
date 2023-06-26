using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderGrenadeLauncher : MonoBehaviour
{
    [SerializeField] private GameObject grenade;
    [SerializeField] private float damage;
    [SerializeField] private float grenadeSpeed;

    public  void DoShot()
    {
        Grenade newGrenade = Instantiate(grenade, transform.position, transform.rotation).GetComponent<Grenade>();
        newGrenade.SetSpeed(grenadeSpeed);
    }
    
}
