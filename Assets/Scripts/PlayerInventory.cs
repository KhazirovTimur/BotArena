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
        Pooler = FindObjectOfType<ObjectPooler>();
        if(WeaponsPrefabs.Count != 0)
        {
            for (int i = 0;  i < WeaponsPrefabs.Count; i++)
            {
                GameObject weapon = Instantiate(WeaponsPrefabs[ActiveWeaponIndex], WeaponRoot);
                weapon.transform.SetParent(WeaponRoot);
                if (weapon.transform.TryGetComponent<AbstractWeapon>(out AbstractWeapon weaponScript))
                {
                    Weapons.Add(weaponScript);
                    if (!weaponScript.IsRayCast)
                    {
                        weaponScript.ProjectilePoolIndex = Pooler.CreatePool(weaponScript.Projectile,
                            (int) (2 / weaponScript.ShotDelay));
                    }
                }
                else
                {
                    Debug.LogError("Weapon prefab in inventory doesn't have AbstractWeapon script");
                }
                WeaponsGO.Add(weapon);
                weapon.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("No weapons prefabs attached to inventory");
        }
    }
    
    
    //Instantiate weapons ammo
    void Start()
    {
        ActiveWeaponIndex = 0;

        for (int i = 0;
             i < System.Enum.GetValues(typeof(AmmoTypes.Ammotypes)).Length;
             i++)
        { 
            WeaponsAmmo.Add(0);
        }
        
        SetActiveWeapon(ActiveWeaponIndex);
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



    public void SetActiveWeapon(int weaponIndex)
    {
        foreach (var VARIABLE in WeaponsGO)
        {
            VARIABLE.SetActive(false);
        }
        WeaponsGO[weaponIndex].SetActive(true);
    }


    
    public void TriggerPushed(bool triggerStatePushed, Vector3 pointOnTarget)
    { 
        Weapons[ActiveWeaponIndex].TriggerPushed(triggerStatePushed, pointOnTarget);
    }
}
