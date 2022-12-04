using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlModel : ObjectModel
{
    [SerializeField] private Animator animator;
    [SerializeField] private TowerModel towerModel;
    public bool IsCollected;

    public override void Initialize()
    {
        base.Initialize();
    }

    public void OnSpawn(Vector3 position, Quaternion rotation, int animationIndex) 
    {
        SetActive();
        transform.position = position;
        transform.rotation = rotation;
    }

    public void OnCollect() 
    {
        if (IsCollected) return;
        IsCollected = true;
        transform.SetParent(towerModel.transform);
        towerModel.Add(this);
        animator.Play("A_Backflip", 0, 0);
        transform.rotation = Quaternion.identity;
    }

    private void onHitObstacle() 
    {
        towerModel.Remove(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "GirlModel":
                GirlModel collectedGirl = other.GetComponent<GirlModel>();
                collectedGirl.OnCollect();
                break;

            case "Diamond":
                Debug.Log("Diamond Collected");
                DiamondModel diamond = other.GetComponent<DiamondModel>();
                diamond.OnCollect();
                break;

            case "Obstacle":
                Debug.Log("Girl hit Obstacle");
                onHitObstacle();
                break;

            default:
                break;
        }
    }
}