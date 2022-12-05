using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : ControllerBaseModel
{
    [SerializeField] private TowerFloorController towerFloorController;
    [SerializeField] private FinishRoadModel finishRoadModel;
    private int scorePlatformIndex = 0;
    private float waitTime = 0.35f;

    public void OnFinish()
    {
        GameController.ChangeState(GameStates.End);
        CameraController.Instance.ChangeCamera(1);
        StartCoroutine(startPlacement());
    }

    private IEnumerator startPlacement()
    {
        if (towerFloorController.FloorSix.Count > 0)
        {
            for (int i = 0; i < towerFloorController.FloorSix.Count; i++)
            {
                GirlModel girl = towerFloorController.FloorSix[i];
                girl.OnScorePlatformPlacement(finishRoadModel.GetPoint(scorePlatformIndex));
            }
            scorePlatformIndex++;
        }
        yield return new WaitForSeconds(waitTime);
        if (towerFloorController.FloorFive.Count > 0)
        {
            for (int i = 0; i < towerFloorController.FloorFive.Count; i++)
            {
                GirlModel girl = towerFloorController.FloorFive[i];
                girl.OnScorePlatformPlacement(finishRoadModel.GetPoint(scorePlatformIndex));
            }
            scorePlatformIndex++;
        }
        yield return new WaitForSeconds(waitTime);
        if (towerFloorController.FloorFour.Count > 0)
        {
            for (int i = 0; i < towerFloorController.FloorFour.Count; i++)
            {
                GirlModel girl = towerFloorController.FloorFour[i];
                girl.OnScorePlatformPlacement(finishRoadModel.GetPoint(scorePlatformIndex));
            }
            scorePlatformIndex++;
        }
        yield return new WaitForSeconds(waitTime);
        if (towerFloorController.FloorThree.Count > 0)
        {
            for (int i = 0; i < towerFloorController.FloorThree.Count; i++)
            {
                GirlModel girl = towerFloorController.FloorThree[i];
                girl.OnScorePlatformPlacement(finishRoadModel.GetPoint(scorePlatformIndex));
            }
            scorePlatformIndex++;
        }
        yield return new WaitForSeconds(waitTime);
        if (towerFloorController.FloorTwo.Count > 0)
        {
            for (int i = 0; i < towerFloorController.FloorTwo.Count; i++)
            {
                GirlModel girl = towerFloorController.FloorTwo[i];
                girl.OnScorePlatformPlacement(finishRoadModel.GetPoint(scorePlatformIndex));
            }
            scorePlatformIndex++;
        }
        yield return new WaitForSeconds(waitTime);
        if (towerFloorController.FloorOne.Count > 0)
        {
            for (int i = 0; i < towerFloorController.FloorOne.Count; i++)
            {
                GirlModel girl = towerFloorController.FloorOne[i];
                girl.OnScorePlatformPlacement(finishRoadModel.GetPoint(scorePlatformIndex));
            }
            scorePlatformIndex++;
        }
        yield return new WaitForSeconds(waitTime);

        //GameController.ChangeState(GameStates.Win);
    }
}