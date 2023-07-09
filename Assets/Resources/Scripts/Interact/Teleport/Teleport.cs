using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour, IInteractable
{

    [SerializeField] private AllLocations.Locations locationName;
    public AllLocations.Locations GetLocationName => locationName;
    [SerializeField] private Teleport teleportTo;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private int teleportCost;
    public Transform GetSpawnTransform => spawnTransform;
    private static bool isEnabled = true;


    
    public void OnInteraction(PlayerInterface client)
    {
        if (isEnabled && client.GetPlayerInventory.GetMoney() >= teleportCost)
        {
            client.GetPlayerTransform.transform.position = teleportTo.GetSpawnTransform.position;
            client.GetPlayerTransform.rotation = teleportTo.GetSpawnTransform.rotation;
            client.GetPlayerStatus.SetCurrentLocation(teleportTo.GetLocationName);
            client.GetPlayerInventory.AddValue(-teleportCost);
        }
    }

    public string GetInteractionLabel()
    {
        if (isEnabled)
            return "Teleport to " + teleportTo.GetLocationName + " for " + teleportCost + " Cells";
        return "Wave in progress. Teleport is disabled";
    }

    public static void DisableTeleports()
    {
        isEnabled = false;
    }
    
    public static void EnableTeleports()
    {
        isEnabled = true;
    }



}
