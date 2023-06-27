using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Contains players weapons, ammo and money

public class PlayerInventory : MonoBehaviour, ICollector
{
    
    
    [Serializable]
    struct PlayerAmmo
    {
         public WeaponsEnums.Ammotypes type;
         public int maxAmmo;
    }
    
    [SerializeField] private List<PlayerAmmo> _playerAmmoTypesWithMaxAmmount;

    private Dictionary<WeaponsEnums.Ammotypes, int> _maxAmmo = new Dictionary<WeaponsEnums.Ammotypes, int>();
    private Dictionary<WeaponsEnums.Ammotypes, int> _currentAmmo = new Dictionary<WeaponsEnums.Ammotypes, int>();
    
    
    [Tooltip("Fill here weapons prefabs")] 
    [SerializeField] private List<GameObject> WeaponsPrefabs;

    //Player hands. Weapon spawns here
    [SerializeField] private WeaponSwitcher WeaponRoot;

    [SerializeField] private AbstractWeapon shoulderGrenadeLauncher;
     
    
    //Cache for weapons scripts components
    private List<AbstractWeapon> Weapons = new List<AbstractWeapon>();

    private Dictionary<AbstractWeapon, bool> _weaponIsUnlocked = new Dictionary<AbstractWeapon, bool>();
    
    //Which weapon is active now
    private int ActiveWeaponIndex;
    private int ActiveWeaponToSet;
    
    private bool canShoot;
    public bool IfCanShoot => canShoot;
    
    public Action ActiveWeaponAmmoChanged;
    public Action WeaponWasChanged;
    public Action MoneyAmountChanged;

    private int money;

    
    private void Start()
    {
        if (CheckPrefabsAssigned())
        {
            InstantiateWeapons();
        }
        ConvertAmmoStructToDictionary();
        ActiveWeaponIndex = 0;
        canShoot = true;
        LeaveOneActiveWeapon(ActiveWeaponIndex);
        UnlockWeapon(Weapons[ActiveWeaponIndex].GetWeapon);
        AddValue(1000);
        AddAmmoToAllWeapons();
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

    private void ConvertAmmoStructToDictionary()
    {
        for(int i = 0; i < _playerAmmoTypesWithMaxAmmount.Count; i ++)
        {
            _maxAmmo.Add(_playerAmmoTypesWithMaxAmmount[i].type, _playerAmmoTypesWithMaxAmmount[i].maxAmmo);
            _currentAmmo.Add(_playerAmmoTypesWithMaxAmmount[i].type, 0);
        }
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


    //Debug func
    private void AddAmmoToAllWeapons()
    {
        var keys = new List<WeaponsEnums.Ammotypes>(_currentAmmo.Keys);
        foreach (var key in keys)
        {
            _currentAmmo[key] += 150;
        }
    }


    //Function for weapons to let them reduce ammo
    public void ReduceActiveWeaponAmmoByShot(int amount)
    {
        _currentAmmo[Weapons[ActiveWeaponIndex].GetWeaponAmmoType] -= amount;
        ActiveWeaponAmmoChanged?.Invoke();
    }


    //Weapons check if ammo left
    public int GetAmmo(WeaponsEnums.Ammotypes ammoType)
    {
        return _currentAmmo[ammoType];
    }
    
    public bool AddAmmo(WeaponsEnums.Ammotypes type, int amount)
    {
        if (_currentAmmo[type] < _maxAmmo[type])
        {
            _currentAmmo[type] += amount;
            ClampAmountOfAmmo(type);
            ActiveWeaponAmmoChanged();
            return true;
        }
        return false;
    }

    private void ClampAmountOfAmmo(WeaponsEnums.Ammotypes type)
    {
        if (_currentAmmo[type] > _maxAmmo[type])
            _currentAmmo[type] = _maxAmmo[type];
    }

    public int GetActiveWeaponAmmo()
    {
        return _currentAmmo[Weapons[ActiveWeaponIndex].GetWeaponAmmoType];
    }

    private void LeaveOneActiveWeapon(int weaponIndex)
    {
        foreach (var weapon in Weapons)
        {
            weapon.GetGameObject().SetActive(false);
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
        MoneyAmountChanged?.Invoke();
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

    public void ShootGrenade(bool shootGrenade)
    {
       shoulderGrenadeLauncher.TriggerPushed(shootGrenade, shoulderGrenadeLauncher.transform.forward);
    }


}
