using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class AttackPlayer : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 lastPlayerPosition;
    private float distanceToPlayer;

    public float projectileSpeed;
    public GameObject projectile;
    public float delayBetweenShots = 3.0f;

    private float projectileTravelTime;
    private float shotTimer;
    public float delayUpdateLastPlayerPos = 0.05f;

    private float timerUpdateLastPlayerPos;

    private ObjectPooler _pooler;
    private string poolIndex = "SimpleEnemy";
    
    

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = FindObjectOfType<FirstPersonController>().transform;
        shotTimer = delayBetweenShots;
        _pooler = FindObjectOfType<ObjectPooler>();
        _pooler.CreatePool(projectile.GetComponent<IPoolable>(), 50, poolIndex);
        timerUpdateLastPlayerPos = delayUpdateLastPlayerPos;
    }

    
    private void Update()
    {
        if (shotTimer <= Time.time)
        {
            if (CanShoot())
            {
                Shoot();
            }
            UpdateShotTimer();
        }

        if (timerUpdateLastPlayerPos <= Time.time)
        {
            UpdatePlayerPosition();
            timerUpdateLastPlayerPos = Time.time + delayUpdateLastPlayerPos;
        }
    }
    

    private bool CanShoot()
    {
        bool canShot = false;
        //Debug.Log("scanning");
        //Debug.DrawLine(transform.position + transform.up, playerTransform.position + playerTransform.up, Color.red, 1.0f);
        if (Physics.Raycast(transform.position + transform.up, playerTransform.position - transform.position + playerTransform.up,
                out RaycastHit hit, 1000))
        {
           // Debug.Log("See smth");
           if (hit.transform.TryGetComponent<PlayerHealth>(out PlayerHealth target))
            {
                canShot = true;
                distanceToPlayer = hit.distance;
                //Debug.Log("i see you");
            }
        }
        else
        {
           // Debug.Log("Doesn't hit((");
        }
        return canShot;
    }
    
    
    

    private void Shoot()
    {
        IPoolable project = _pooler.GetPool(poolIndex).Get();
        project.GetGameObject().transform.position = transform.position + transform.up;
        project.GetGameObject().transform.LookAt(PredictPlayerPosition());
        IProjectile bullet = project.GetGameObject().GetComponent<IProjectile>();
        bullet.SetSpeed(projectileSpeed);
        
    }

    private Vector3 PredictPlayerPosition()
    {
        projectileTravelTime = distanceToPlayer / projectileSpeed;
        return playerTransform.position + playerTransform.up + (GetPlayerDirection().normalized * GetPlayerSpeed() * projectileTravelTime);
    }

    private void UpdatePlayerPosition()
    { 
        lastPlayerPosition = playerTransform.position;
    }

    private Vector3 GetPlayerDirection()
    {
        return playerTransform.position - lastPlayerPosition;
    }

    private float GetPlayerSpeed()
    {
        return GetPlayerDirection().magnitude / delayUpdateLastPlayerPos;
    }

    private void UpdateShotTimer()
    {
        shotTimer = Time.time + delayBetweenShots;
    }



}
