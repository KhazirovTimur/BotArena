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
    public WeaponSwitcher WeaponRoot;

    //Which weapon is active now
    public int ActiveWeaponIndex;

    private int ActiveWeaponToSet;

    private bool canShoot;
    public bool IfCanShoot => canShoot;
    
    public Action ActiveWeaponAmmoReduced;
    public Action WeaponWasChanged;
    
    private int money;

    public Action MoneyAmountChanged;

    

    
    //Instantiate weapons
    void Start()
    {
        if (CheckPrefabsAssigned())
        {
            InstantiateWeapons();
            AddAmmoToAllWeapons();
        }
        ActiveWeaponIndex = 0;
        canShoot = true;
        LeaveOneActiveWeapon(ActiveWeaponIndex);
        WeaponRoot.WeaponWasLowered += ChangeWeaponPrefab;
        WeaponRoot.WeaponIsReady += FinishWeaponChange;
        WeaponWasChanged?.Invoke();
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
            GameObject weapon = Instantiate(WeaponsPrefabs[i], WeaponRoot.transform);
            weapon.transform.SetParent(WeaponRoot.transform);
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
    public void ReduceAmmoByShot()
    {
        int ammoIndex = (int)(Weapons[ActiveWeaponIndex].GetWeaponAmmoType);
        WeaponsAmmo[ammoIndex] -= 1;
        ActiveWeaponAmmoReduced();
    }
    
    
    //Weapons check if ammo left
    public int GetAmmo(int ammoIndex)
    {
        return WeaponsAmmo[ammoIndex];
    }
    
    public int GetActiveWeaponAmmo()
    {
        int ammoIndex = (int)(Weapons[ActiveWeaponIndex].GetWeaponAmmoType);
        return WeaponsAmmo[ammoIndex];
    }
    

    private void LeaveOneActiveWeapon(int weaponIndex)
    {
        foreach (var VARIABLE in Weapons)
        {
            VARIABLE.GetGameObject().SetActive(false);
        }
        Weapons[weaponIndex].GetGameObject().SetActive(true);
        //WeaponWasChanged();
    }


    public void StartWeaponChange(int weaponIndex)
    {
        if (weaponIndex < Weapons.Count && ActiveWeaponIndex != weaponIndex)
        {
            WeaponRoot.SetTrueNeedToChangeWeapon();
            canShoot = false;
            ActiveWeaponToSet = weaponIndex;
        }
    }

    public void ChangeWeaponPrefab()
    {
        Weapons[ActiveWeaponIndex].GetGameObject().SetActive(false);
        Weapons[ActiveWeaponToSet].GetGameObject().SetActive(true);
        ActiveWeaponIndex = ActiveWeaponToSet;
        WeaponWasChanged();
    }

    public void FinishWeaponChange()
    {
        canShoot = true;
    }


    public void TriggerPushed(bool triggerStatePushed, Vector3 pointOnTarget)
    { 
        Weapons[ActiveWeaponIndex].TriggerPushed(triggerStatePushed, pointOnTarget);
    }

    public void AddMoney(int ToAdd)
    {
        money += ToAdd;
        MoneyAmountChanged();
    }

    public int GetMoney()
    {
        return 0;
    }
}
