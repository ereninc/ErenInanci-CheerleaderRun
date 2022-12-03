using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using DG.Tweening;

public class FormationController : ControllerBaseModel
{
    [SerializeField] private List<GirlModel> girlModels;
    [SerializeField] GirlModel prefab;

    //public int rows = 3;
    //public float rowOffset = -.5f;
    //public float yOffset = -1f;
    //public float xOffset = 1f;
    private void Start()
    {
        girlModels= new List<GirlModel>();
    }

    [EditorButton]
    public void Add(/*GirlModel model*/)
    {
        GirlModel gM = Instantiate(prefab);
        girlModels.Add(gM);
        SetFormation();
    }

    public void Remove(GirlModel model)
    {
        if (girlModels.Count > 0)
        {
            girlModels.Remove(model);
        }
    }

    [SerializeField] List<Vector3> positions;

    [EditorButton]
    public void SetFormation()
    {
        //Vector3 targetPos = Vector3.left;
        //for (int i = 0; i <= rows; i++)
        //{
        //    for (int j = 0; j < i; j++)
        //    {
        //        GameObject gO = Instantiate(prefab);
        //        gO.transform.SetParent(transform);
        //        targetPos = new Vector3(targetPos.x + xOffset, targetPos.y, 0);
        //        gO.transform.position = targetPos;
        //    }
        //    targetPos = new Vector3((rowOffset * i) - xOffset,targetPos.y + yOffset);
        //}


        for (int i = 0; i < girlModels.Count; i++)
        {
            girlModels[i].transform.DOMove(positions[i], 0.15f);
        }
    }

    [EditorButton]
    public void E_GetItems()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            girlModels.Add(transform.GetChild(i).GetComponent<GirlModel>());
        }
    }
}