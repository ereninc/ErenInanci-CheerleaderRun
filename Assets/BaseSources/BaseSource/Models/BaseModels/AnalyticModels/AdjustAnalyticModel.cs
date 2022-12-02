using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if Adjust
using com.adjust.sdk;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CubeGames.Analytic
{
    public class AdjustAnalyticModel : AnalyticBaseModel
    {
#if Adjust
        [SerializeField] Adjust adjustPrefab;
#endif
        [Header("Event Tokens")] [SerializeField]
        string levelStartedToken;

        [SerializeField] string levelSuccesToken;
        [SerializeField] string levelFailToken;

        public override string DefinationSymbol()
        {
            return "Adjust";
        }

        public override void OnAddAnalytic()
        {
            base.OnAddAnalytic();
            createPrefab();
        }

        public override void RemoveAnalytic()
        {
            base.RemoveAnalytic();
#if Adjust
            if (adjustPrefab != null)
                DestroyImmediate(adjustPrefab.gameObject);
#endif
        }

        public override void E_Editor()
        {
#if UNITY_EDITOR && Adjust
            base.E_Editor();

            EditorGUILayout.Space();

            if (adjustPrefab == null)
            {
                if (GUILayout.Button("Create Adjust Prefab"))
                {
                    createPrefab();
                }
            }
            else
            {
                EditorGUILayout.LabelField("Analytic Settings");
                adjustPrefab.appToken = EditorGUILayout.TextField("App Token", adjustPrefab.appToken);
                adjustPrefab.environment =
 (AdjustEnvironment)EditorGUILayout.EnumPopup("Environment", adjustPrefab.environment);
            }
#endif
        }

        private void createPrefab()
        {
#if UNITY_EDITOR && Adjust
            adjustPrefab = (Adjust)AssetDatabase.LoadAssetAtPath("Assets/Adjust/Prefab/Adjust.prefab", typeof(Adjust));
            adjustPrefab = (Adjust)PrefabUtility.InstantiatePrefab(adjustPrefab);
            adjustPrefab.transform.parent = transform;
#endif
        }

        public override void OnLevelCompleted(int level)
        {
            base.OnLevelCompleted(level);
#if Adjust
            AdjustEvent adjustEvent = new AdjustEvent(levelSuccesToken);
            Adjust.trackEvent(adjustEvent);
#endif
        }

        public override void OnLevelFailed(int level)
        {
            base.OnLevelFailed(level);
#if Adjust
            AdjustEvent adjustEvent = new AdjustEvent(levelFailToken);
            Adjust.trackEvent(adjustEvent);
#endif
        }

        public override void OnLevelStarted(int level)
        {
            base.OnLevelStarted(level);
#if Adjust
            AdjustEvent adjustEvent = new AdjustEvent(levelStartedToken);
            Adjust.trackEvent(adjustEvent);
#endif
        }

        public void SendCustomEvent(string token, string key, string value)
        {
            base.SendCustomEvent(key, value);
#if Adjust
            AdjustEvent adjustEvent = new AdjustEvent(token);
            adjustEvent.addCallbackParameter(key,value);
            Adjust.trackEvent(adjustEvent);
#endif
        }
    }
}