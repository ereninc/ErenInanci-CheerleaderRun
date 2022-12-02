using System;
using UnityEngine;
#if GA
using GameAnalyticsSDK;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CubeGames.Analytic
{
#if GA
    using GameAnalyticsSDK;
#endif

    public class GAAnalyticModel : AnalyticBaseModel
    {
        private string lastAndroidGameKey, lastAndroidSecretKey;
        private string lastIosGameKey, lastIosSecretKey;

        public override void Initialize()
        {
            base.Initialize();
#if GA
            GameAnalytics.Initialize();
#endif
        }

        public override void OnLevelStarted(int level)
        {
            base.OnLevelStarted(level);
#if GA
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level_" + level.ToString());
#endif
        }

        public override void OnLevelCompleted(int level)
        {
            base.OnLevelCompleted(level);
#if GA
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level_" + level.ToString());
#endif
        }

        public override void OnLevelFailed(int level)
        {
            base.OnLevelFailed(level);
#if GA
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level_" + level.ToString());
#endif
        }

        public void SendCustomEvent(string key, float value)
        {
            base.SendCustomEvent(key, value);
#if GA
            GameAnalytics.NewDesignEvent(key, value);
#endif
        }

        public override string DefinationSymbol()
        {
            return "GA";
        }

        public override void OnAddAnalytic()
        {
            base.OnAddAnalytic();

#if UNITY_EDITOR && GA
            if (GameObject.FindObjectOfType(typeof(GameAnalytics)) == null)
            {
                GameObject go =
                    PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath(
                        GameAnalytics.WhereIs("GameAnalytics.prefab", "Prefab"), typeof(GameObject))) as GameObject;
                go.name = "GameAnalytics";
                Selection.activeObject = go;
                Undo.RegisterCreatedObjectUndo(go, "Created GameAnalytics Object");
            }
#endif
        }

        public override void RemoveAnalytic()
        {
            base.RemoveAnalytic();

#if GA
            GameAnalytics ga = GameObject.FindObjectOfType<GameAnalytics>();
            if (ga != null)
                DestroyImmediate(ga.gameObject);
#endif
        }

        public override void E_Editor()
        {
#if UNITY_EDITOR && GA
            base.E_Editor();

            EditorGUILayout.Space(10);


            if (GameAnalytics.SettingsGA.Platforms.Exists(x => x == RuntimePlatform.Android))
            {
                EditorGUILayout.LabelField("Android");
                int index = GameAnalytics.SettingsGA.Platforms.FindIndex(x => x == RuntimePlatform.Android);
                GameAnalytics.SettingsGA.UpdateGameKey(index,
                    EditorGUILayout.TextField("GameKey", GameAnalytics.SettingsGA.GetGameKey(index)));
                GameAnalytics.SettingsGA.UpdateSecretKey(index,
                    EditorGUILayout.TextField("SecretKey", GameAnalytics.SettingsGA.GetSecretKey(index)));
                GameAnalytics.SettingsGA.Build[index] =
                    EditorGUILayout.TextField("Build", GameAnalytics.SettingsGA.Build[index]);

                if (!String.Equals(lastAndroidGameKey, GameAnalytics.SettingsGA.GetGameKey(index)) ||
                    !String.Equals(lastAndroidSecretKey, GameAnalytics.SettingsGA.GetSecretKey(index)))
                {
                    EditorUtility.SetDirty(GameAnalytics.SettingsGA);
                    AssetDatabase.SaveAssets();
                }

                lastAndroidGameKey = GameAnalytics.SettingsGA.GetGameKey(index);
                lastAndroidSecretKey = GameAnalytics.SettingsGA.GetSecretKey(index);
            }
            else
            {
                if (GUILayout.Button("Add Android Platform"))
                {
                    GameAnalytics.SettingsGA.AddPlatform(RuntimePlatform.Android);
                }
            }

            EditorGUILayout.Space(10);
            if (GameAnalytics.SettingsGA.Platforms.Exists(x => x == RuntimePlatform.IPhonePlayer))
            {
                EditorGUILayout.LabelField("iOS");
                int index = GameAnalytics.SettingsGA.Platforms.FindIndex(x => x == RuntimePlatform.IPhonePlayer);
                GameAnalytics.SettingsGA.UpdateGameKey(index,
                    EditorGUILayout.TextField("GameKey", GameAnalytics.SettingsGA.GetGameKey(index)));
                GameAnalytics.SettingsGA.UpdateSecretKey(index,
                    EditorGUILayout.TextField("SecretKey", GameAnalytics.SettingsGA.GetSecretKey(index)));
                GameAnalytics.SettingsGA.Build[index] =
                    EditorGUILayout.TextField("Build", GameAnalytics.SettingsGA.Build[index]);
                if (!String.Equals(lastIosGameKey, GameAnalytics.SettingsGA.GetGameKey(index)) ||
                    !String.Equals(lastIosSecretKey, GameAnalytics.SettingsGA.GetSecretKey(index)))
                {
                    EditorUtility.SetDirty(GameAnalytics.SettingsGA);
                    AssetDatabase.SaveAssets();
                }


                lastIosGameKey = GameAnalytics.SettingsGA.GetGameKey(index);
                lastIosSecretKey = GameAnalytics.SettingsGA.GetSecretKey(index);
            }
            else
            {
                if (GUILayout.Button("Add iOS Platform"))
                {
                    GameAnalytics.SettingsGA.AddPlatform(RuntimePlatform.IPhonePlayer);
                }
            }


#endif
        }
    }
}