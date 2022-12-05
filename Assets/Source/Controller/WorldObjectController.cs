using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectController : ControllerBaseModel
{
    [SerializeField] private PoolModel girlPool;
    [SerializeField] private PoolModel collectablePool;
    [SerializeField] private PoolModel obstaclePool;

    [SerializeField] private LevelController levelController;
    private LevelModel activeLevel;

    public override void Initialize()
    {
        base.Initialize();
        activeLevel = levelController.ActiveLevel;
        loadWorldItems();
    }

    private void loadWorldItems()
    {
        for (int i = 0; i < activeLevel.GirlData.Length; i++)
        {
            GirlModel girl = girlPool.GetDeactiveItem<GirlModel>();
            GirlDataModel dataModel = activeLevel.GirlData[i];
            girl.transform.position = dataModel.Position;
            girl.transform.rotation = dataModel.Rotation;
            girl.SetActive();
        }

        for (int i = 0; i < activeLevel.CollectableData.Length; i++)
        {
            DiamondModel diamond = collectablePool.GetDeactiveItem<DiamondModel>();
            CollectableDataModel dataModel = activeLevel.CollectableData[i];
            diamond.transform.position = dataModel.Position;
            diamond.SetActive();
        }

        for (int i = 0; i < activeLevel.ObstacleData.Length; i++)
        {
            ObstacleModel obstacle = obstaclePool.GetDeactiveItem<ObstacleModel>();
            ObstacleDataModel dataModel = activeLevel.ObstacleData[i];
            obstacle.transform.position = dataModel.Position;
            obstacle.SetActive();
        }
    }
}
