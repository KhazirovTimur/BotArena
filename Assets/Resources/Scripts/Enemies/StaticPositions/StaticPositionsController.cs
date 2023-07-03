using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class StaticPositionsController : MonoBehaviour
{

    private List<StaticPosition> _availablePositions = new List<StaticPosition>();
    

    public void RemoveFromAvailablePositions(StaticPosition position)
    {
        _availablePositions.Remove(position);
        Debug.Log($"Removed point, point count {_availablePositions.Count}");
    }

    public void AddAvailablePosition(StaticPosition position)
    {
        _availablePositions.Add(position);
        Debug.Log($"Added point, point count {_availablePositions.Count}");
    }

    public bool HavePositions()
    {
        if (_availablePositions.Count > 0)
            return true;
        return false;
    }

    public StaticPosition GetAvailablePosition()
    {
        return _availablePositions[0];
    }

}
