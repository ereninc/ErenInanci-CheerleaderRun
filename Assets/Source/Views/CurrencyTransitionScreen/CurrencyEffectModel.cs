using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyEffectModel : ObjectModel
{
    public void OnSpawn(Vector3 pos, Vector3 scale) 
    {
        SetActive();
        transform.position = pos;
        transform.localScale = scale;
    }

    public void OnDeactive() 
    {
        SetDeactive();
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
    }
}