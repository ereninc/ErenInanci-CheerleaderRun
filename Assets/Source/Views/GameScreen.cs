using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : ScreenElement
{
    [SerializeField] private TextMeshProUGUI money;

    public override void Show()
    {
        base.Show();
        UpdateMoneyBar();
    }

    public void UpdateMoneyBar()
    {
        money.text = IncomeController.Instance.GetIncome().ToCoinValues();
    }
}