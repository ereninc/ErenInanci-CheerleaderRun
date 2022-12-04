using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using DG.Tweening;

public class TowerModel : ObjectModel
{
    [SerializeField] private List<GirlModel> girlModels;
    [SerializeField] float xOffset = 1.5f;
    [SerializeField] float yOffset = 3.75f;
    public float CameraXOffset;

    public void Add(GirlModel girlModel)
    {
        girlModels.Add(girlModel);
        SetFormation();
    }

    public void Remove(GirlModel model)
    {
        if (girlModels.Count > 0)
        {
            model.SetDeactive();
            girlModels.Remove(model);
            SetFormation();
        }
    }

    [EditorButton]
    public void SetFormation()
    {
        Vector3 Origin = Vector3.zero;
        int currentCheerleader = 1;
        int currentColumnNumber = 1;
        int cheerleadersInCurrentColumn;
        while (currentCheerleader <= girlModels.Count)
        {
            cheerleadersInCurrentColumn = 1;
            Vector3 position = new Vector3(Origin.x + (currentColumnNumber - 1) * xOffset, Origin.y);
            while (cheerleadersInCurrentColumn <= currentColumnNumber && currentCheerleader <= girlModels.Count)
            {
                girlModels[currentCheerleader - 1].transform.SetParent(transform);
                girlModels[currentCheerleader - 1].transform.DOLocalMove(position, 0.15f);
                position += new Vector3(-xOffset / 2, yOffset, 0);
                cheerleadersInCurrentColumn++;
                currentCheerleader++;
            }
            currentColumnNumber++;
            CameraXOffset = currentColumnNumber * 0.5f; //Tower's bottom-middle position
        }

        transform.DOLocalMoveX(-CameraXOffset, 0.25f); //Set every move
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