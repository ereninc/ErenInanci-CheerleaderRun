using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class FormationController : ControllerBaseModel
{
    [SerializeField] private List<GirlModel> girlModels;
    [SerializeField] GameObject prefab;
    public int rows = 3;
    public float rowOffset = -.5f;
    public float yOffset = -1f;
    public float xOffset = 1f;

    public void Add(GirlModel model)
    {
        girlModels.Add(model);
    }

    public void Remove(GirlModel model)
    {
        if (girlModels.Count > 0)
        {
            girlModels.Remove(model);
        }
    }

    [EditorButton]
    public void SetFormation()
    {
        Vector3 targetPos = Vector3.left;

        for (int i = 0; i <= rows; i++)
        {
            for (int j = 0; j < i; j++)
            {
                GameObject gO = Instantiate(prefab);
                gO.transform.SetParent(transform);
                targetPos = new Vector3(targetPos.x + xOffset, targetPos.y, 0);
                gO.transform.position = targetPos;
            }
            targetPos = new Vector3((rowOffset * i) - xOffset,targetPos.y + yOffset);
        }
    }
}