using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//[System.Serializable]
public class TowerFloorController : ControllerBaseModel
{
    public int MaxFloorCount = 6;
    public List<GirlModel> FloorOne;
    public List<GirlModel> FloorTwo;
    public List<GirlModel> FloorThree;
    public List<GirlModel> FloorFour;
    public List<GirlModel> FloorFive;
    public List<GirlModel> FloorSix;

    public override void Initialize()
    {
        base.Initialize();
        setFloors();
    }

    public void Place(int index, GirlModel girl)
    {
        switch (index)
        {
            case 1:
                if (!FloorOne.Contains(girl)) FloorOne.Add(girl);
                break;
            case 2:
                if (!FloorTwo.Contains(girl)) FloorTwo.Add(girl);
                break;
            case 3:
                if (!FloorThree.Contains(girl)) FloorThree.Add(girl);
                break;
            case 4:
                if (!FloorFour.Contains(girl)) FloorFour.Add(girl);
                break;
            case 5:
                if (!FloorFive.Contains(girl)) FloorFive.Add(girl);
                break;
            case 6:
                if (!FloorSix.Contains(girl)) FloorSix.Add(girl);
                break;
            default:
                break;
        }
    }

    public void ResetFloors() 
    {
        setFloors();
    }

    private void setFloors()
    {
        FloorOne = new List<GirlModel>();
        FloorTwo = new List<GirlModel>();
        FloorThree = new List<GirlModel>();
        FloorFour = new List<GirlModel>();
        FloorFive = new List<GirlModel>();
        FloorSix = new List<GirlModel>();
    }
}