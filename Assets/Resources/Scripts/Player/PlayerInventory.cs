using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Contains players weapons and ammo

public class PlayerInventory : MonoBehaviour, ICollector
{

    [Tooltip("Fill here weapons prefabs")] public List<GameObject> WeaponsPrefabs;

    //Cache for weapons scripts components
    [HideInInspector] public List<AbstractWeapon> Weapons = new List<AbstractWeapon>();

    private Dictionary<AbstractWeapon, bool> _weaponIsUnlocked = new Dictionary<AbstractWeapon, bool>();

    //List of ammo for all weapons
    public List<int> WeaponsAmmo = new List<int>();

    //Player hands. Weapon spawns here
    public WeaponSwitcher WeaponRoot;

    //Which weapon is active now
    public int ActiveWeaponIndex;

    private int ActiveWeaponToSet;

    private bool canShoot;
    public bool IfCanShoot => canShoot;

    public Action ActiveWeaponAmmoChanged;
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
        UnlockWeapon(Weapons[ActiveWeaponIndex].GetWeapon);
        AddValue(1000);
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

        for (int i = 0; i < WeaponsPrefabs.Count; i++)
        {
            GameObject weapon = Instantiate(WeaponsPrefabs[i], WeaponRoot.transform);
            weapon.transform.SetParent(WeaponRoot.transform);
            if (weapon.transform.TryGetComponent(out AbstractWeapon weaponScript))
            {
                Weapons.Add(weaponScript);
                _weaponIsUnlocked.Add(weaponScript, false);
                weaponScript.InitializeWeapon();
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
             i < System.Enum.GetValues(typeof(WeaponsEnums.Ammotypes)).Length;
             i++)
        {
            WeaponsAmmo.Add(150);
        }
    }


    //Function for weapons to let them reduce ammo
    public void ReduceAmmoByShot()
    {
        int ammoIndex = (int) (Weapons[ActiveWeaponIndex].GetWeaponAmmoType);
        WeaponsAmmo[ammoIndex] -= 1;
        ActiveWeaponAmmoChanged();
    }


    //Weapons check if ammo left
    public int GetAmmo(int ammoIndex)
    {
        return WeaponsAmmo[ammoIndex];
    }
    
    public bool AddAmmo(WeaponsEnums.Ammotypes type, int ammount)
    {
        WeaponsAmmo[(int) type] += ammount;
        ActiveWeaponAmmoChanged();
        return true;
    }

    public int GetActiveWeaponAmmo()
    {
        int ammoIndex = (int) (Weapons[ActiveWeaponIndex].GetWeaponAmmoType);
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


    public void StartWeaponChange(int weaponIndex)
    {
        if (weaponIndex > Weapons.Count)
            return;
        if (ActiveWeaponIndex == weaponIndex)
            return;
        if (_weaponIsUnlocked[Weapons[weaponIndex]] == false)
            return;
        WeaponRoot.SetTrueNeedToChangeWeapon();
        canShoot = false;
        ActiveWeaponToSet = weaponIndex;
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

    public void AddValue(int value)
    {
        money += value;
        MoneyAmountChanged();
    }

    public int GetMoney()
    {
        return money;
    }

    public bool UnlockWeapon(WeaponsEnums.Weapons weapon)
    {
        foreach (var weaponInst in Weapons)
        {
            if (weaponInst.GetWeapon == weapon)
            {
                if (!_weaponIsUnlocked[weaponInst])
                {
                    _weaponIsUnlocked[weaponInst] = true;
                    StartWeaponChange(Weapons.IndexOf(weaponInst));
                    return true;
                }

                return AddAmmo(weaponInst.GetWeaponAmmoType, 100);
            }
        }

        return false;
    }


}
