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

    public void OnInteraction(PlayerInterface Client)
    {
        if(Client.GetPlayerInventory.GetMoney() < cost)
            return;
        if(Client.GetPlayerInventory.UnlockWeapon(weaponForSale))
            Client.GetPlayerInventory.AddValue(-cost);
    }

    public string GetInteractionLabel()
    {
        string text = "Buy " + weaponForSale + " for " + cost + " cells";
        return text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
