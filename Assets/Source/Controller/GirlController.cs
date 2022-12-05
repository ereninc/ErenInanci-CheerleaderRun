using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlController : ControllerBaseModel
{
    [SerializeField] private TowerModel towerModel;
    [SerializeField] private LevelController levelController;
    [SerializeField] private PoolModel girlPool;
    private LevelModel activeLevel;
    private int girlCount;

    public override void Initialize()
    {
        base.Initialize();
        activeLevel = levelController.ActiveLevel;
        girlCount = activeLevel.StartGirlCount;
        setStarterGirlAnimations();
    }

    private void setStarterGirlAnimations()
    {
        for (int i = 0; i < girlCount; i++)
        {
            GirlModel girl = girlPool.GetDeactiveItem<GirlModel>();
            girl.OnLevelSpawn(towerModel.transform);
            towerModel.Add(girl);
        }

        for (int i = 0; i < towerModel.GirlModels.Count; i++)
        {
            GirlModel girlModel = towerModel.GirlModels[i];
            girlModel.Initialize();
        }
    }

    public void OnLevelStartAnimation()
    {
        for (int i = 0; i < towerModel.GirlModels.Count; i++)
        {
            GirlModel girlModel = towerModel.GirlModels[i];
            girlModel.SetTowerAnimation();
        }
    }
}