using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using DG.Tweening;

public class FormationController : ControllerBaseModel
{
    [SerializeField] private List<GirlModel> girlModels;
    [SerializeField] GirlModel prefab;
    [SerializeField] private float xOffset = 1.5f;
    [SerializeField] private float yOffset = 3.75f;

    private void Start()
    {
        girlModels = new List<GirlModel>();
    }

    [EditorButton]
    public void Add(/*GirlModel model*/)
    {
        GirlModel gM = Instantiate(prefab);
        girlModels.Add(gM);
        SetFormation();
    }

    public void Add(GirlModel girlModel)
    {
        girlModels.Add(girlModel);
        SetFormation();
    }

    [EditorButton]
    public void Remove(GirlModel model)
    {
        if (girlModels.Count > 0)
        {
            model.SetDeactive();
            girlModels.Remove(model);
            SetFormation();
        }
    }

    [SerializeField] List<Vector3> positions;

    [EditorButton]
    public void SetFormation()
    {
        //Vector3 targetPosition = Vector3.zero;
        //girlModels[0].transform.position = targetPosition;
        //for (int i = 1; i < girlModels.Count + 1; i++)
        //{
        //    targetPosition = new Vector3(xOffset, yOffset, 0);
        //    girlModels[i].transform.position = targetPosition;
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