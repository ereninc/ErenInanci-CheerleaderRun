using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondModel : CollectableBaseModel
{
    public override void Initialize()
    {
        base.Initialize();
    }

    [EditorButton]
    public override void OnCollect()
    {
        CurrencyTransitionController.Instance.EmitParticlesInTime(1, .15f, Camera.main.WorldToScreenPoint(transform.position));
        base.OnCollect();
    }
}