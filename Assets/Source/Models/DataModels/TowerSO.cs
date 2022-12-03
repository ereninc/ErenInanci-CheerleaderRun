using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataModelSO/TowerPositionData")]
public class TowerSO : ScriptableObject
{
    public List<Vector3> TowerPositions;
}