using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StaticPosition : MonoBehaviour
{
    [SerializeField] private LayerMask occulingLayers;
    [SerializeField] private float timeToActivatePosition = 2.0f;
    [SerializeField] private float timeToDeactivatePosition = 5.0f;

    private bool _positionIsActive;
    private bool _positionIsOccupied;
    
    private StaticPositionsController _parentController;

    private Transform _playerTransform;

    private float _timer;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        _parentController = GetComponentInParent<StaticPositionsController>();
        _playerTransform = FindObjectOfType<FirstPersonController>().transform;
        _positionIsActive = false;
        _positionIsOccupied = false;
        _timer = timeToActivatePosition;
    }

    // Update is called once per frame
    private void Update()
    {
        if(_positionIsOccupied)
            return;
        if(_positionIsActive)
            CheckDeactivatePositionTimer();
        else
        {
            CheckActivatePositionTimer();
        }
    }

    private void CheckActivatePositionTimer()
    {
        if (_timer <= 0)
        {
            _parentController.AddAvailablePosition(this);
            _timer =  timeToDeactivatePosition;
            _positionIsActive = true;
            return;
        }
        if (!Physics.Linecast(transform.position + Vector3.up, _playerTransform.position + Vector3.up, occulingLayers))
        {
            _timer -= Time.deltaTime;
            return;
        }
        _timer = timeToActivatePosition;
    }

    private void CheckDeactivatePositionTimer()
    {
        if (_timer <= 0)
        {
            _parentController.RemoveFromAvailablePositions(this);
            _timer = timeToActivatePosition;
            _positionIsActive = false;
            return;
        }
        if (Physics.Linecast(transform.position + Vector3.up, _playerTransform.position + Vector3.up, occulingLayers))
        {
            _timer -= Time.deltaTime;
            return;
        }
        _timer = timeToDeactivatePosition;
    }

    public Transform OccupyPosition()
    {
        _positionIsOccupied = true;
        _parentController.RemoveFromAvailablePositions(this);
        return transform;
    }

    public void FreePosition()
    {
        _positionIsOccupied = false;
    }

}
