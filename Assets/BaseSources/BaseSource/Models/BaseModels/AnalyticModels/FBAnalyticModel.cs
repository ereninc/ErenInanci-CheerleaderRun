using System;

namespace CubeGames.Analytic
{
#if FB
    using Facebook.Unity;
    using Facebook.Unity.Settings;
#endif
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class FBAnalyticModel : AnalyticBaseModel
    {
        private string lastName, lastId;

        public override void Initialize()
        {
            base.Initialize();
#if FB
            FB.Init(() =>
                 {
                     FB.ActivateApp();
                 });
#endif
        }

        public override string DefinationSymbol()
        {
            return "FB";
        }

        public override void E_Editor()
        {
            base.E_Editor();

#if UNITY_EDITOR && FB
            EditorGUILayout.LabelField("Settings");
            var appLabel = EditorGUILayout.TextField("App Name", FacebookSettings.AppLabels[0]);
            FacebookSettings.AppLabels[0] = appLabel;
            var appId= EditorGUILayout.TextField("App Id", FacebookSettings.AppIds[0]);
            FacebookSettings.AppIds[0] =appId;
            if (!String.Equals(lastName ,FacebookSettings.AppLabels[0]) || !String.Equals(lastId,
                FacebookSettings.AppIds[0]))
            {
                EditorUtility.SetDirty(Facebook.Unity.Settings.FacebookSettings.Instance);
                AssetDatabase.SaveAssets();
                Debug.Log("ID Changed");
            }

            lastName = FacebookSettings.AppLabels[0];
            lastId = FacebookSettings.AppIds[0];
#endif
        }
    }
}