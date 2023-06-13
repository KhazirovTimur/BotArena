using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyWeaponObject : MonoBehaviour, IInteractable
{
    [SerializeField] private WeaponsEnums.Weapons weaponForSale;
    [SerializeField] private int cost;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnInteraction(PlayerInventory Client)
    {
        if(Client.GetMoney() < cost)
            return;
        if(Client.UnlockWeapon(weaponForSale))
            Client.AddValue(-cost);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
