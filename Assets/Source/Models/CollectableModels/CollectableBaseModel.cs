using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBaseModel : ObjectModel
{
    public void OnSpawn(Vector3 pos)
    {
        SetActive();
        transform.position = pos;
    }

    public virtual void OnCollect() 
    {
        SetDeactive();
    }
}