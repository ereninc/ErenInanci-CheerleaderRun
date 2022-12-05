using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModel : ScriptableObject
{
    [Range(1, 5)]
    public int StartGirlCount;
    public GirlDataModel[] GirlData;
    public CollectableDataModel[] CollectableData;
    public ObstacleDataModel[] ObstacleData;
}