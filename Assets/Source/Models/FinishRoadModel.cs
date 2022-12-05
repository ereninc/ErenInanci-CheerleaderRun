using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishRoadModel : ObjectModel
{
    [SerializeField] private List<Transform> scorePlatformPositions;

    public int GetPointCount()
    {
        return scorePlatformPositions.Count;
    }

    public Transform GetPoint(int index)
    {
        return scorePlatformPositions[index];
    }
}