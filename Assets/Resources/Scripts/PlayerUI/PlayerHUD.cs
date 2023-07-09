using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerHUD : MonoBehaviour
{

    
    [SerializeField] private TextMeshProUGUI amountOfAmmo;
    [SerializeField] private TextMeshProUGUI amountOfMoney;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private TextMeshProUGUI playerHP;
    [SerializeField] private TextMeshProUGUI restTimer;
    [SerializeField] private TextMeshProUGUI wavesCount;
    [SerializeField] private float labelDisappearTime = 0.1f;
    [SerializeField] private PlayerInput playerInput;

    private EnemyWavesController _enemyWavesController;
    
    private PlayerInterface _player;
    
    private bool _needToReset;
    private float _labelDisappearTimer;

    private float _restTimerTime;
    

    private void Awake()
    {
        _player = FindObjectOfType<PlayerInterface>();
        playerInput = FindObjectOfType<PlayerInput>();
        _enemyWavesController = FindObjectOfType<EnemyWavesController>();
        SetListeners();
    }

    private void SetListeners()
    {
        _player.GetPlayerInventory.WeaponWasChanged += UpdateAmountOfAmmo;
        _player.GetPlayerInventory.ActiveWeaponAmmoChanged += UpdateAmountOfAmmo;
        _player.GetPlayerInventory.MoneyAmountChanged += UpdateAmountOfMoney;
        _player.GetPlayerStatus.UpdateHP += () => { playerHP.text = _player.GetPlayerStatus.GetCurrentHp().ToString("0");};
        FindObjectOfType<FirstPersonController>().LookAtInteractable += UpdateInteractText;
        _enemyWavesController.WaveWasEnded += StartRestTimer;
        _enemyWavesController.WaveWasEnded += () =>
        {
            wavesCount.text = _enemyWavesController.GetSurvivedWaves.ToString();
        };
    }

    private void Update()
    {
        if (_labelDisappearTimer < Time.time && _needToReset)
        {
            ResetLabels();
            _needToReset = false;
        }
        UpdateRestTimer();
    }


    private void UpdateAmountOfAmmo()
    {
        amountOfAmmo.text = _player.GetPlayerInventory.GetActiveWeaponAmmo().ToString();
    }

    private void UpdateAmountOfMoney()
    {
        amountOfMoney.text = _player.GetPlayerInventory.GetMoney().ToString();
    }

    private void UpdateInteractText(IInteractable otherObject)
    {
        interactionText.text = otherObject.GetInteractionLabel();
        _labelDisappearTimer = Time.time + labelDisappearTime;
        _needToReset = true;
    }
    
    private void StartRestTimer()
    {
        _restTimerTime = _enemyWavesController.GetRestTimer;
        restTimer.gameObject.SetActive(true);
    }

    private void UpdateRestTimer()
    {
        if (_restTimerTime <= 0)
        {
            restTimer.gameObject.SetActive(false);
            return;
        }
        restTimer.text = _restTimerTime.ToString("0.0");
        _restTimerTime -= Time.deltaTime;
    }

    private void ResetLabels()
    {
        interactionText.text = "";
    }

}
