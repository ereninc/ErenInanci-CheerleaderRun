using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlController : ControllerBaseModel
{
    [SerializeField] private TowerModel towerModel;

    public override void Initialize()
    {
        base.Initialize();
        setStarterGirlAnimations();
    }

    private void setStarterGirlAnimations()
    {
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