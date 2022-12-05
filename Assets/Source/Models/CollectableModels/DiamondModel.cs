using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondModel : CollectableBaseModel
{
    [SerializeField] private int Amount = 10;

    public override void Initialize()
    {
        base.Initialize();
    }

    [EditorButton]
    public override void OnCollect()
    {
        IncomeController.Instance.IncreaseIncome(Amount);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        CurrencyTransitionController.Instance.EmitParticlesInTime(1, .15f, screenPos);
        base.OnCollect();
    }

    public CollectableDataModel GetDataModel()
    {
        CollectableDataModel dataModel = new CollectableDataModel();
        dataModel.Position = transform.position;

        return dataModel;
    }
}