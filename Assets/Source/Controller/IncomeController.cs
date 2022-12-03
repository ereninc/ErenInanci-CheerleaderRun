using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeController : ControllerBaseModel
{
    public static IncomeController Instance;
    public Action onIncomeUpdate;

    public override void Initialize()
    {
        base.Initialize();
        if (Instance!=null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void IncreaseIncome(int amount) 
    {
        PlayerDataModel.Data.Money += amount;
        PlayerDataModel.Data.Save();
    }

    public int GetIncome()
    {
        return PlayerDataModel.Data.Money;
    }
}