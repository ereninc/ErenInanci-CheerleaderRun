using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if AppsFlyer
using AppsFlyerSDK;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CubeGames.Analytic
{
    public class AppsFlyerAnalyticModel : AnalyticBaseModel
    {
#if AppsFlyer
        public AppsFlyerObjectScript appsFlyerInstance;
#endif
        private int devKey, appId, UWPAppId;

        public override string DefinationSymbol()
        {
            return "AppsFlyer";
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnAddAnalytic()
        {
            base.OnAddAnalytic();
            createPrefab();
        }

        public override void RemoveAnalytic()
        {
            base.RemoveAnalytic();
#if UNITY_EDITOR && AppsFlyer
            if (appsFlyerInstance != null)
                DestroyImmediate(appsFlyerInstance.gameObject);
#endif
        }

        public override void OnLevelStarted(int level)
        {
            base.OnLevelStarted(level);
#if AppsFlyer
            Dictionary<string, string> levelStart = new Dictionary<string, string>();
            levelStart.Add("Level", level.ToString());
            AppsFlyer.sendEvent("LevelStart", levelStart);
#endif
        }

        public override void OnLevelCompleted(int level)
        {
            base.OnLevelCompleted(level);
#if AppsFlyer
            Dictionary<string, string> levelComplete = new Dictionary<string, string>();
            levelComplete.Add("Level", level.ToString());
            AppsFlyer.sendEvent("LevelComplete", levelComplete);
#endif
        }

        public override void OnLevelFailed(int level)
        {
            base.OnLevelFailed(level);
#if AppsFlyer
            Dictionary<string, string> levelFail = new Dictionary<string, string>();
            levelFail.Add("Level", level.ToString());
            AppsFlyer.sendEvent("LevelFail", levelFail);
#endif
        }

        public void SendCustomEvent(string key, Dictionary<string, string> value)
        {
            base.SendCustomEvent(key, value);
#if AppsFlyer
            AppsFlyer.sendEvent(key, value);
#endif
        }

        private void createPrefab()
        {
#if UNITY_EDITOR && AppsFlyer
            AppsFlyerObjectScript appsFlyer =
 (AppsFlyerObjectScript)AssetDatabase.LoadAssetAtPath("Assets/AppsFlyer/AppsFlyerObject.prefab", typeof(AppsFlyerObjectScript));
            appsFlyerInstance = (AppsFlyerObjectScript)PrefabUtility.InstantiatePrefab(appsFlyer);
            appsFlyerInstance.transform.parent = transform;
#endif
        }

        public override void E_Editor()
        {
#if UNITY_EDITOR && AppsFlyer
            base.E_Editor();
            EditorGUILayout.Space();

            if (appsFlyerInstance == null)
            {
                if (GUILayout.Button("Create Appsflyer Prefab"))
                {
                    createPrefab();
                }
            }
            else
            {
                EditorGUILayout.LabelField("Analytic Settings");
                appsFlyerInstance.devKey = EditorGUILayout.TextField("Dev Key", appsFlyerInstance.devKey);
                appsFlyerInstance.appID = EditorGUILayout.TextField("App Id", appsFlyerInstance.appID);
                appsFlyerInstance.UWPAppID = EditorGUILayout.TextField("UWP App Id", appsFlyerInstance.UWPAppID);
                appsFlyerInstance.isDebug = EditorGUILayout.Toggle("Is Debug", appsFlyerInstance.isDebug);
                appsFlyerInstance.getConversionData =
                    EditorGUILayout.Toggle("Get Conversion Data", appsFlyerInstance.getConversionData);

                if (!String.Equals(devKey, appsFlyerInstance.devKey) ||
                    !String.Equals(appId, appsFlyerInstance.appID) ||
                    !String.Equals(UWPAppId, appsFlyerInstance.UWPAppID))
                {
                    EditorUtility.SetDirty(appsFlyerInstance);
                    AssetDatabase.SaveAssets();
                }
            }
#endif
        }
    }
}