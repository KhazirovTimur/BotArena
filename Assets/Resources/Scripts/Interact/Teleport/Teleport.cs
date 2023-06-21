using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour, IInteractable
{

    [SerializeField] private string locationName;
    public string GetLocationName => locationName;
    [SerializeField] private Teleport teleportTo;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private int teleportCost;
    public Transform GetSpawnTransform => spawnTransform;
    private static bool isEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void OnInteraction(PlayerInterface client)
    {
        if (isEnabled && client.GetPlayerInventory.GetMoney() > teleportCost)
        {
            client.GetPlayerTransform.transform.position = teleportTo.GetSpawnTransform.position;
            client.GetPlayerTransform.rotation = teleportTo.GetSpawnTransform.rotation;
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
