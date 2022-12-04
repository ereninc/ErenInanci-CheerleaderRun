using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterModel : ObjectModel
{
    public override void Initialize()
    {
        base.Initialize();
        transform.localPosition = Vector3.zero;
    }

    public void OnLevelFinished()
    {
        GameController.ChangeState(GameStates.End);
        transform.DOLocalMoveX(0, 0.15f);
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