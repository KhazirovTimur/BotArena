using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerHUD : MonoBehaviour
{

    
    [SerializeField] private TextMeshProUGUI amountOfAmmo;
    [SerializeField] private TextMeshProUGUI amountOfMoney;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private TextMeshProUGUI playerHP;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private float labelDisappearTime = 0.1f;
    [SerializeField] private PlayerInput playerInput;


    private bool needToReset = false;
    private float _disappearTimer;
    

    private void Awake()
    {
        if (!playerInventory)
            playerInventory = FindObjectOfType<PlayerInventory>();
        playerInput = FindObjectOfType<PlayerInput>();
        SetListeners();
    }

    private void SetListeners()
    {
        playerInventory.WeaponWasChanged += UpdateAmountOfAmmo;
        playerInventory.ActiveWeaponAmmoChanged += UpdateAmountOfAmmo;
        playerInventory.MoneyAmountChanged += UpdateAmountOfMoney;
        FindObjectOfType<FirstPersonController>().LookAtInteractable += UpdateInteractText;
    }

    private void Update()
    {
        if (_disappearTimer < Time.time && needToReset)
        {
            ResetLabels();
            needToReset = false;
        }
    }


    private void UpdateAmountOfAmmo()
    {
        amountOfAmmo.text = playerInventory.GetActiveWeaponAmmo().ToString();
    }

    private void UpdateAmountOfMoney()
    {
        amountOfMoney.text = playerInventory.GetMoney().ToString();
    }

    private void UpdateInteractText(IInteractable otherObject)
    {
        interactionText.text = otherObject.GetInteractionLabel();
        _disappearTimer = Time.time + labelDisappearTime;
        needToReset = true;
    }

    private void ResetLabels()
    {
        interactionText.text = "";
    }

}
