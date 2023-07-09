using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    private NavMeshAgent agent;

    private Transform player;
    private float timer;
    private Action _destinationReached;

    private bool _onTheMoveToPlayer = false;
    private float _distance;
    
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        player = FindObjectOfType<FirstPersonController>().transform;
    }


    private void Update()
    {
        if (_onTheMoveToPlayer)
        {
            CheckDistanceToPlayer();
        }
        if((agent.destination - agent.transform.position).magnitude < 0.2f)
            Stop();
    }


    public void MoveToPlayerAtDistance(float distance)
    {
        agent.destination = player.position;
        _onTheMoveToPlayer = true;
        _distance = distance;
    }
    

    private void CheckDistanceToPlayer()
    {
        if((transform.position - player.position).magnitude < _distance)
            _destinationReached?.Invoke();
    }

    public void MoveToPoint(Vector3 destination)
    {

        agent.destination = destination;
    }

    public void Stop()
    {
        agent.ResetPath();

    }


}
