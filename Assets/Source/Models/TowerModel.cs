using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using DG.Tweening;

public class TowerModel : ObjectModel
{
    [SerializeField] private TowerSO TowerData;
    [SerializeField] private List<GirlModel> girlModels;

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
        for (int i = 0; i < girlModels.Count; i++)
        {
            girlModels[i].SetActive();
            girlModels[i].transform.localPosition = TowerData.TowerPositions[i];
        }
    }

    [EditorButton]
    public void E_GetItems()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).SetActiveGameObject(false);
            girlModels.Add(transform.GetChild(i).GetComponent<GirlModel>());
        }
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