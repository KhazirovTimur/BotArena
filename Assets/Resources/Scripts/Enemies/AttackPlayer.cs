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

    [SerializeField] private LayerMask occulingLayers;
    public float projectileSpeed;
    public GameObject projectile;
    public float delayBetweenShots = 3.0f;

    private float projectileTravelTime;
    private float shotTimer;
    public float delayUpdateLastPlayerPos = 0.05f;

    private float timerUpdateLastPlayerPos;

    private ObjectPoolContainer objectPoolContainer;

    
    
    

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = FindObjectOfType<FirstPersonController>().transform;
        shotTimer = delayBetweenShots;
        //if(!objectPoolContainer)
           // objectPoolContainer = FindObjectOfType<AllObjectPoolsContainer>()
             //   .CreateNewPool(projectile.GetComponent<IPoolable>(), 50);
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
        if (Physics.Linecast(transform.position + Vector3.up, playerTransform.position + Vector3.up, occulingLayers))
        {
            return false;
        }
        distanceToPlayer = (playerTransform.position - transform.position).magnitude;
        return true;
    }
    

    private void Shoot()
    {
        IPoolable project = objectPoolContainer.GetPool.Get();
        project.GetGameObject().transform.position = transform.position + transform.up;
        project.GetGameObject().transform.LookAt(PredictPlayerPosition());
        IProjectile bullet = project.GetGameObject().GetComponent<IProjectile>();
        bullet.SetSpeed(projectileSpeed);
        
    }


    public void SetObjectPoolContainer(ObjectPoolContainer poolContainer)
    {
        objectPoolContainer = poolContainer;
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
