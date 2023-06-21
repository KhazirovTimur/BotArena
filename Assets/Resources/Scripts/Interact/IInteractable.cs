using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void OnInteraction(PlayerInterface client);

    public string GetInteractionLabel();
}
