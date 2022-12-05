using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class TowerFloorController : ControllerBaseModel
{
    public int MaxFloorCount = 6;
    public TowerModel TowerModel;
    public FinishRoadModel FinishRoadModel;
    private int index = 0;


    public override void Initialize()
    {
        base.Initialize();
    }

    public void OnLevelEnded()
    {
        StartCoroutine(checkFloors());
    }

    private IEnumerator checkFloors()
    {
        List<GirlModel> floorGirls = new List<GirlModel>();
        for (int i = MaxFloorCount; i > 0; i--)
        {
            for (int j = 0; j < TowerModel.GirlModels.Count; j++)
            {
                GirlModel girl = TowerModel.GirlModels[j];
                if (girl.GetHeight() == i)
                {
                    floorGirls.Add(girl);
                }
            }
            if (floorGirls.Count > 0)
            {
                for (int k = 0; k < floorGirls.Count; k++)
                {
                    GirlModel girl = floorGirls[k];
                    girl.OnScorePlatformPlacement(FinishRoadModel.GetPoint(FinishRoadModel.GetPointCount() - i + index));
                }
            }
            else
            {
                index--;
            }
            floorGirls.Clear();
            yield return new WaitForSeconds(0.25f);
        }
    }
}