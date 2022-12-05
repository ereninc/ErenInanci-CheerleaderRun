using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using DG.Tweening;

public class TowerModel : ObjectModel
{
    [SerializeField] private TowerFloorController floorController;
    [SerializeField] float xOffset = 1.5f;
    [SerializeField] float yOffset = 3.75f;
    private float towerXPosition;
    public List<GirlModel> GirlModels;

    public override void Initialize()
    {
        base.Initialize();
        floorController.Initialize();
    }

    public void Add(GirlModel girlModel)
    {
        GirlModels.Add(girlModel);
        SetFormation();
    }

    public void Remove(GirlModel model)
    {
        if (GirlModels.Count > 0)
        {
            //model.SetDeactive();
            ResetTower();
            GirlModels.Remove(model);
            Invoke(nameof(SetFormation), 0.35f);
        }
    }

    [EditorButton]
    public void SetFormation()
    {
        Vector3 Origin = Vector3.zero;
        int currentCheerleader = 1;
        int currentColumnNumber = 1;
        int cheerleadersInCurrentColumn;
        while (currentCheerleader <= GirlModels.Count)
        {
            cheerleadersInCurrentColumn = 1;
            Vector3 position = new Vector3(Origin.x + (currentColumnNumber - 1) * xOffset, Origin.y);
            while (cheerleadersInCurrentColumn <= currentColumnNumber && currentCheerleader <= GirlModels.Count)
            {
                GirlModel girl = GirlModels[currentCheerleader - 1];
                girl.transform.DOLocalMove(position, 0.25f);
                int floor = girl.OnTowerPlacement(position.y);
                floorController.Place(floor, girl);
                position += new Vector3(-xOffset / 2, yOffset, 0);
                cheerleadersInCurrentColumn++;
                currentCheerleader++;
            }
            currentColumnNumber++;
            towerXPosition = currentColumnNumber * 0.5f; //Tower's bottom-middle position
        }

        transform.DOLocalMoveX(-towerXPosition, 0.25f); //Set every move
    }

    public void ResetTower() 
    {
        floorController.ResetFloors();
    }
}

/*
  PLACEMENT RULE
              ..
              ..
              ..
           9.  ...
         5.   8.  ...
       2.   4.   7.  ...
     0.   1.   3.   6.  ...
*/