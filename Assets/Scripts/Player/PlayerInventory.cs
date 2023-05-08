using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Contains players weapons and ammo

public class PlayerInventory : MonoBehaviour
{

    [Tooltip("Fill here weapons prefabs")]
    public List<GameObject> WeaponsPrefabs;
    
    //Cache for weapons gameobjects
    [HideInInspector]
    public List<GameObject> WeaponsGO = new List<GameObject>();
    
    //Cache for weapons scripts components
    [HideInInspector]
    public List<AbstractWeapon> Weapons = new List<AbstractWeapon>();
    
    //List of ammo for all weapons
    public List<int> WeaponsAmmo = new List<int>();
    
    //Player hands. Weapon spawns here
    public Transform WeaponRoot;

    //Which weapon is active now
    public int ActiveWeaponIndex;

    //Cache for object pooler
    public ObjectPooler Pooler;
    


    //Ask Object pooler to create pools for all non-rayCast weapons
    //Instantiate all weapons
    private void Awake()
    {

    }
    
    
    //Instantiate weapons ammo
    void Start()
    {
        if (CheckPrefabsAssigned())
        {
            InstantiateWeapons();
            CreateAmmoPools();
            AddAmmoToAllWeapons();
        }
        
        ActiveWeaponIndex = 0;
        LeaveOneActiveWeapon(ActiveWeaponIndex);
    }
    
    private bool CheckPrefabsAssigned()
    {
        if (WeaponsPrefabs.Count != 0)
            return true;
        Debug.LogError("No weapons prefabs attached to inventory");
        return false;
    }


    private void InstantiateWeapons()
    {
        
        for (int i = 0;  i < WeaponsPrefabs.Count; i++)
        {
            GameObject weapon = Instantiate(WeaponsPrefabs[i], WeaponRoot);
            weapon.transform.SetParent(WeaponRoot);
            if (weapon.transform.TryGetComponent(out AbstractWeapon weaponScript))
            {
                Weapons.Add(weaponScript);


            }
            else
            {
                Debug.LogError("Weapon prefab in inventory doesn't have AbstractWeapon script");
            }
            WeaponsGO.Add(weapon);
            weapon.SetActive(true);
        }
    }

    private void CreateAmmoPools()
    {
        Pooler = FindObjectOfType<ObjectPooler>();
        foreach (var weapon in Weapons)
        {
            AbstractWeapon weaponScript = weapon.GetComponent<AbstractWeapon>();
            if (!weaponScript.IsRayCast)
            { 
                Pooler.CreatePool(weaponScript.Projectile.GetComponent<IPoolable>(),
                    (int) (2 / weaponScript.ShotDelay), weaponScript.Name);
                weaponScript.ProjectilePoolIndex = weaponScript.Name;
            }
        }

    }


    private void AddAmmoToAllWeapons()
    {
        for (int i = 0;
             i < System.Enum.GetValues(typeof(AmmoTypes.Ammotypes)).Length;
             i++)
        { 
            WeaponsAmmo.Add(150);
        }
    }


    //Function for weapons to let them reduce ammo
    public void ReduceAmmoByOne()
    {
        int ammoIndex = (int)(Weapons[ActiveWeaponIndex].WeaponAmmoType);
        WeaponsAmmo[ammoIndex] -= 1;
    }
    
    
    //Weapons check if ammo left
    public int GetAmmo(int ammoIndex)
    {
        return WeaponsAmmo[ammoIndex];
    }
    

    private void LeaveOneActiveWeapon(int weaponIndex)
    {
        foreach (var VARIABLE in WeaponsGO)
        {
            VARIABLE.SetActive(false);
        }
        WeaponsGO[weaponIndex].SetActive(true);
    }


    public void ChangeActiveWeapon(int weaponIndex)
    {
        if (weaponIndex < Weapons.Count)
        {
            WeaponsGO[ActiveWeaponIndex].SetActive(false);
            ActiveWeaponIndex = weaponIndex;
            WeaponsGO[ActiveWeaponIndex].SetActive(true);
        }
    }




    public void TriggerPushed(bool triggerStatePushed, Vector3 pointOnTarget)
    { 
        Weapons[ActiveWeaponIndex].TriggerPushed(triggerStatePushed, pointOnTarget);
    }
}
