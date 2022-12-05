using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlVisualModel : ObjectModel
{
    [SerializeField] private Material onCollectedMaterial;
    [SerializeField] private List<Renderer> renderers;

    public void OnCollect() 
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material = onCollectedMaterial;
        }
    }
}
