using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidStore : MonoBehaviour, IInteractable
{
    [SerializeField] private int cost;

    public void OnInteraction(PlayerInterface client)
    {
        if (client.GetPlayerInventory.GetMoney() > cost)
        {
            client.GetPlayerStatus.ResetHP();
            client.GetPlayerInventory.AddValue(-cost);
        }
    }

    public string GetInteractionLabel()
    {
        string text = $"Restore HP {cost} cells";
        return text;
    }
}
