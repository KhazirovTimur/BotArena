using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{

    
    [SerializeField]
    private TextMeshProUGUI amountOfAmmo;
    [SerializeField]
    private TextMeshProUGUI amountOfMoney;
    [SerializeField]
    private TextMeshProUGUI playerHP;
    [SerializeField]
    private PlayerInventory playerInventory;
    

    private void Awake()
    {
        if (!playerInventory)
            playerInventory = FindObjectOfType<PlayerInventory>();
        SetListeners();
    }

    private void SetListeners()
    {
        playerInventory.WeaponWasChanged += UpdateAmountOfAmmo;
        playerInventory.ActiveWeaponAmmoReduced += UpdateAmountOfAmmo;
        playerInventory.MoneyAmountChanged += UpdateAmountOfMoney;
    }


    private void UpdateAmountOfAmmo()
    {
        amountOfAmmo.text = playerInventory.GetActiveWeaponAmmo().ToString();
    }

    private void UpdateAmountOfMoney()
    {
        amountOfMoney.text = playerInventory.GetMoney().ToString();
    }
}
