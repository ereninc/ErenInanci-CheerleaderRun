using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CubeGames.Analytic;

[CustomEditor(typeof(AnalyticBaseModel),true)]
public class AnalyticModelEditor : Editor
{
    AnalyticBaseModel model
    {
        get
        {
            return (AnalyticBaseModel)target;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


#if UNITY_EDITOR
        if (EditorApplication.isCompiling)
        {
            GUILayout.Label("Compiling..");
        }
        else
        {
            if (GUILayout.Button("Remove Analytic"))
            {
                model.RemoveAnalytic();
            }

            model.E_Editor();
        }
#endif

      
    }
}
