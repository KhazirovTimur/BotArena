using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradeStore : MonoBehaviour, IInteractable
{
    [SerializeField] private int cost;
    

    public void OnInteraction(PlayerInterface client)
    {
        if (client.GetPlayerInventory.GetMoney() > cost)
        {
            client.GetPlayerInventory.UpgradeCurrentWeapon(1.1f);
            client.GetPlayerInventory.AddValue(-cost);
        }
        
    }

    public string GetInteractionLabel()
    {
        string text = $"Upgrade weapon damage on 10% for {cost} cells";
        return text;
    }
}
