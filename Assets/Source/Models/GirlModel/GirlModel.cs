using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlModel : ObjectModel
{
    [SerializeField] private Animator animator;
    [SerializeField] private TowerModel towerModel;
    [SerializeField] private int heightInTower;
    [SerializeField] private GirlVisualModel girlVisualModel;
    [SerializeField] private Collider col;
    public bool IsCollected;

    public override void Initialize()
    {
        base.Initialize();
        girlVisualModel.OnCollect();
    }

    public void OnLevelSpawn(Transform towerParent)
    {
        SetActive();
        transform.SetParent(towerParent);
        IsCollected = true;
    }

    public void OnCollected()
    {
        if (IsCollected) return;
        IsCollected = true;
        transform.SetParent(towerModel.transform);
        towerModel.Add(this);
        setOnCollectAnimation();
        transform.rotation = Quaternion.identity;
        girlVisualModel.OnCollect();
        Invoke(nameof(SetTowerAnimation), 0.85f);
    }

    public void OnScorePlatformPlacement(Transform targetPoint)
    {
        animator.Play("A_RunningFlip", 0, 0);
        transform.DOJump(new Vector3(transform.position.x,
            targetPoint.position.y,
            targetPoint.position.z), 10, 1, 0.25f).OnComplete(() => transform.Rotate(0, 180, 0));
    }

    public int OnTowerPlacement(float height)
    {
        if (height > 0)
        {
            heightInTower = (int)(height / 3.75f) + 1;
        }
        else
        {
            heightInTower = 1;
        }
        return heightInTower;
    }

    public int GetHeight()
    {
        return heightInTower;
    }

    private void onHitObstacle()
    {
        towerModel.Remove(this);
        transform.SetParent(null);
        animator.Play("A_GetHitBackward", 0, 0);
        transform.DOLocalMoveY(0, 0.15f);
        col.enabled = false;
    }

    private void setOnCollectAnimation()
    {
        if (heightInTower > 1) animator.Play("A_Backflip", 0, 0);
        else animator.Play("A_Running", 0, 0);

    }

    public void SetTowerAnimation()
    {
        if (heightInTower > 1) animator.Play("A_Cheering", 0, 0);
        else animator.Play("A_Running", 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "GirlModel":
                GirlModel collectedGirl = other.GetComponent<GirlModel>();
                collectedGirl.OnCollected();
                break;
            case "Diamond":
                DiamondModel diamond = other.GetComponent<DiamondModel>();
                diamond.OnCollect();
                break;
            case "Obstacle":
                onHitObstacle();
                break;
            default:
                break;
        }
    }

    public GirlDataModel GetDataModel()
    {
        GirlDataModel dataModel = new GirlDataModel();
        dataModel.Position = transform.position;
        dataModel.Rotation = transform.rotation;

        return dataModel;
    }
}