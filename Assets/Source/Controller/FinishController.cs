using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : ControllerBaseModel
{
    [SerializeField] private TowerFloorController towerFloorController;
    [SerializeField] private FinishRoadModel finishRoadModel;

    public void OnFinish()
    {
        GameController.ChangeState(GameStates.End);
        CameraController.Instance.ChangeCamera(1);
        towerFloorController.OnLevelEnded();
    }
}