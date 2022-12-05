using UnityEngine;

[System.Serializable]
public class WorldItemDataModel
{
    public int Id;
    public Vector3 Position;
    public Quaternion Rotation;
}

[System.Serializable]
public class CollectableDataModel
{
    public Vector3 Position;
}

[System.Serializable]
public class ObstacleDataModel
{
    public Vector3 Position;
}

[System.Serializable]
public class GirlDataModel
{
    public Vector3 Position;
    public Quaternion Rotation;
}