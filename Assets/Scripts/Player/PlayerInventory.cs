using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Contains players weapons and ammo

public class PlayerInventory : MonoBehaviour
{

    [Tooltip("Fill here weapons prefabs")]
    public List<GameObject> WeaponsPrefabs;
    
    //Cache for weapons scripts components
    [HideInInspector]
    public List<AbstractWeapon> Weapons = new List<AbstractWeapon>();
    
    //List of ammo for all weapons
    public List<int> WeaponsAmmo = new List<int>();
    
    //Player hands. Weapon spawns here
    public Transform WeaponRoot;

    //Which weapon is active now
    public int ActiveWeaponIndex;
    

    

    
    //Instantiate weapons
    void Start()
    {
        if (CheckPrefabsAssigned())
        {
            InstantiateWeapons();
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
            weapon.SetActive(true);
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
        int ammoIndex = (int)(Weapons[ActiveWeaponIndex].GetWeaponAmmoType);
        WeaponsAmmo[ammoIndex] -= 1;
    }
    
    
    //Weapons check if ammo left
    public int GetAmmo(int ammoIndex)
    {
        return WeaponsAmmo[ammoIndex];
    }
    

    private void LeaveOneActiveWeapon(int weaponIndex)
    {
        foreach (var VARIABLE in Weapons)
        {
            VARIABLE.GetGameObject().SetActive(false);
        }
        Weapons[weaponIndex].GetGameObject().SetActive(true);
    }


    public void ChangeActiveWeapon(int weaponIndex)
    {
        Debug.Log($"change weapon to {weaponIndex}");
        if (weaponIndex < Weapons.Count)
        {
            Weapons[ActiveWeaponIndex].GetGameObject().SetActive(false);
            ActiveWeaponIndex = weaponIndex;
            Weapons[ActiveWeaponIndex].GetGameObject().SetActive(true);
        }
        Debug.Log($"active weapon is {ActiveWeaponIndex}");
    }




    public void TriggerPushed(bool triggerStatePushed, Vector3 pointOnTarget)
    { 
        Weapons[ActiveWeaponIndex].TriggerPushed(triggerStatePushed, pointOnTarget);
    }
}
