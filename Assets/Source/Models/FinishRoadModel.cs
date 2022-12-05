using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishRoadModel : ObjectModel
{
    [SerializeField] private List<Transform> scorePlatformPositions;

    public Transform GetPoint(int index)
    {
        return scorePlatformPositions[index];
    }
}