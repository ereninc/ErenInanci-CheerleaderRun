using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPositionController : ControllerBaseModel
{
    [SerializeField] private TowerSO TowerData;
    [SerializeField] private Transform TowerTransform;
    [SerializeField] private int maxPositionCount;

    [EditorButton]
    public void CalculatePositions() 
    {
        
    }
}