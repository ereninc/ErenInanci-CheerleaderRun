using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CubeGames.Analytic;
using System;
using System.Reflection;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AnalyticController : ControllerBaseModel
{
    static AnalyticController instance;
    public static bool Initialized;
    [SerializeField] List<AnalyticBaseModel> analytics;

    Type[] analyticModels;

    private void Awake()
    {
        Initialize();
    }

    public override void Initialize()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
#if UNITY_ANDROID || UNITY_IOS
        for (int i = 0; i < analytics.Count; i++)
        {
            analytics[i].Initialize();
        }
#elif UNITY_EDITOR
        print("Analytics - initialized.");
#endif
        Initialized = true;
        base.Initialize();
    }

    public static T GetAnalytic<T>()
    {
        return (T) ((object) instance.analytics.Find(x => x.GetType() == typeof(T)));
    }

    public static void OnLevelStarted(int level)
    {
#if UNITY_ANDROID || UNITY_IOS
        for (int i = 0; i < instance.analytics.Count; i++)
        {
            instance.analytics[i].OnLevelStarted(level);
        }
#elif UNITY_EDITOR
        print("Analytics - On Level Started event is triggered!");
#endif
    }

    public static void OnLevelFailed(int level)
    {
#if UNITY_ANDROID || UNITY_IOS
        for (int i = 0; i < instance.analytics.Count; i++)
        {
            instance.analytics[i].OnLevelFailed(level);
        }
#elif UNITY_EDITOR
        print("Analytics - On Level Failed event is triggered!");
#endif
    }

    public static void OnLevelCompleted(int level)
    {
#if UNITY_ANDROID || UNITY_IOS
        for (int i = 0; i < instance.analytics.Count; i++)
        {
            instance.analytics[i].OnLevelCompleted(level);
        }
#elif UNITY_EDITOR
        print("Analytics - On Level Completed event is triggered!");
#endif
    }

    public void RemoveAnalytic(AnalyticBaseModel analyticBaseModel)
    {
#if UNITY_EDITOR
        removeDefineSymbol(analyticBaseModel.DefinationSymbol(), BuildTargetGroup.Android);
        removeDefineSymbol(analyticBaseModel.DefinationSymbol(), BuildTargetGroup.iOS);
        Undo.RecordObject(this, "Remove_" + analyticBaseModel.name);
#endif
        analytics.Remove(analyticBaseModel);
        DestroyImmediate(analyticBaseModel);
    }

    [EditorCustomInpector]
    public void Editor()
    {
#if UNITY_EDITOR
        if (EditorApplication.isCompiling)
        {
            GUILayout.Label("Compiling..");
        }
        else
        {
            if (GUILayout.Button("Add Analytic"))
            {
                e_showAnalytics();
            }

            if (!Application.isMobilePlatform)
            {
                EditorGUILayout.HelpBox("Target platform isn't mobile", MessageType.Warning);
            }
        }
#endif
    }

    private void e_showAnalytics()
    {
#if UNITY_EDITOR
        analyticModels = getInheritedClasses(typeof(AnalyticBaseModel));

        GenericMenu genericMenu = new GenericMenu();
        for (int i = 0; i < analyticModels.Length; i++)
        {
            Type type = analyticModels[i];
            if (gameObject.GetComponent(type) == null)
            {
                genericMenu.AddItem(new GUIContent(type.ToString()), false, () => addAnalyticModel(type));
            }
        }

        genericMenu.ShowAsContext();
#endif
    }

    private void addAnalyticModel(Type type)
    {
        gameObject.AddComponent(type);
        AnalyticBaseModel analyticBase = (AnalyticBaseModel) gameObject.GetComponent(type);
        string defineSymbol = analyticBase.DefinationSymbol();
#if UNITY_EDITOR
        addDefineSymbol(defineSymbol, BuildTargetGroup.Android);
        addDefineSymbol(defineSymbol, BuildTargetGroup.iOS);
        Undo.RecordObject(this, "add" + type.Name);

#endif
        analyticBase.OnAddAnalytic();
        if (analytics == null)
            analytics = new List<AnalyticBaseModel>();
        analytics.Add(analyticBase);
    }

    Type[] getInheritedClasses(Type MyType)
    {
        return Assembly.GetAssembly(MyType).GetTypes()
            .Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(MyType)).ToArray();
    }

#if UNITY_EDITOR
    private void addDefineSymbol(string symbol, BuildTargetGroup buildTargetGroup)
    {
        List<string> defineSymbols =
            PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';').ToList();
        if (defineSymbols.Contains(symbol))
        {
            return;
        }

        defineSymbols.Add(symbol);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defineSymbols.ToArray());
    }
#endif

#if UNITY_EDITOR
    private void removeDefineSymbol(string symbol, BuildTargetGroup buildTargetGroup)
    {
        List<string> defineSymbols =
            PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';').ToList();
        defineSymbols.Remove(symbol);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defineSymbols.ToArray());
    }
#endif
}