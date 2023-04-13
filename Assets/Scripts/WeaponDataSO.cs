using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 51)]
public class WeaponDataSo : ScriptableObject
{
    [SerializeField]
    private string name;
    
    [SerializeField]
    private float damage;

    [SerializeField]
    private float shotDelay;

    [SerializeField] 
    private int capacity;
    
    [SerializeField] 
    private float reloadTime;
    
    [SerializeField] 
    private bool isFullAuto;
    
    public int Capacity => capacity;

    public string Name => name;

    public float Damage => damage;

    public float ShotDelay => shotDelay;

    public float ReloadTime => reloadTime;

    public bool IsFullAuto => isFullAuto;
}
