using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterModel : ObjectModel
{
    [SerializeField] private FinishController finishController;

    public override void Initialize()
    {
        base.Initialize();
        transform.localPosition = Vector3.zero;
    }

    public void OnLevelFinished()
    {
        transform.DOLocalMoveX(0, 0.15f);
        finishController.OnFinish();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "FinishRoad":
                OnLevelFinished();
                break;
            default:
                break;
        }
    }
}