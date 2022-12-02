using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CubeGames.Tutorial
{
    public class TutorialController : ControllerBaseModel
    {
        public static TutorialController Instance;
        public TutorialScreen tutorialScreen;
        public List<LessonModel> lessons;
        
        public override void Initialize()
        {
            base.Initialize();
            Instance = this;
       
        }

        public void CheckTutorial()
        {
            var tutorialIndex = TutorialDataModel.Data.index;
            if (tutorialIndex < lessons.Count)
            {
                switch (lessons[tutorialIndex].LessonType)
                {
                    case LessonTypes.Swipe:
                    {
                        tutorialScreen.SetActiveLesson(LessonTypes.Swipe);
                        lessons[tutorialIndex].GetComponent<SwipeLessonModel>().Load();
                        break;
                    }
                    case LessonTypes.ClickButton:
                    {
                        tutorialScreen.SetActiveLesson(LessonTypes.ClickButton);
                        lessons[tutorialIndex].GetComponent<ClickButtonLessonModel>().Load(tutorialScreen.transform);
                        break;
                    }
                    case LessonTypes.ClickObject:
                    {
                        tutorialScreen.SetActiveLesson(LessonTypes.ClickObject);
                        lessons[tutorialIndex].GetComponent<ClickObjectLessonModel>().Load();
                        break;
                    }
                    case LessonTypes.TapHold:
                    {
                        tutorialScreen.SetActiveLesson(LessonTypes.TapHold);
                        lessons[tutorialIndex].GetComponent<TapHoldLessonModel>().Load();
                        break;
                    }
                }
            }
       
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TutorialController))]

    public class TutorialHelperEditor : Editor
    {
        public LessonTypes lessonType;
        TutorialController TutorialController
        {
            get
            {
                return (TutorialController)target;
            }
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Color color = GUI.color;
            EditorGUILayout.Space(20);
            lessonType = (LessonTypes)EditorGUILayout.EnumPopup("Lesson Type", lessonType);
        
            GUI.color = Color.green;
            if (GUILayout.Button("Add Lesson"))
            {
                if (lessonType == LessonTypes.TapHold)
                {
                    GameObject lessonModel = new GameObject("Tap&HoldLesson");
                    lessonModel.transform.SetParent(TutorialController.transform);
                    lessonModel.AddComponent<TapHoldLessonModel>();
                    TapHoldLessonModel lesson = lessonModel.GetComponent<TapHoldLessonModel>();
                    var screen = TutorialController.tutorialScreen;
                    lesson.Initialize(lessonType, screen);
                    TutorialController.lessons.Add(lesson);
                }
                if (lessonType == LessonTypes.ClickObject)
                {
                    GameObject lessonModel = new GameObject("ClickObjectLesson");
                    lessonModel.transform.SetParent(TutorialController.transform);
                    lessonModel.AddComponent<ClickObjectLessonModel>();
                    ClickObjectLessonModel lesson = lessonModel.GetComponent<ClickObjectLessonModel>();
                    var screen = TutorialController.tutorialScreen;
                    lesson.Initialize(lessonType, screen);
                    TutorialController.lessons.Add(lesson);
                }
                if (lessonType == LessonTypes.ClickButton)
                {
                    GameObject lessonModel = new GameObject("ClickButtonLesson");
                    lessonModel.transform.SetParent(TutorialController.transform);
                    lessonModel.AddComponent<ClickButtonLessonModel>();
                    ClickButtonLessonModel lesson = lessonModel.GetComponent<ClickButtonLessonModel>();
                    var screen = TutorialController.tutorialScreen;
                    lesson.Initialize(lessonType, screen);
                    TutorialController.lessons.Add(lesson);
                }
                if (lessonType == LessonTypes.Swipe)
                {
                    GameObject lessonModel = new GameObject("SwipeLesson");
                    lessonModel.transform.SetParent(TutorialController.transform);
                    lessonModel.AddComponent<SwipeLessonModel>();
                    SwipeLessonModel lesson = lessonModel.GetComponent<SwipeLessonModel>();
                    var screen = TutorialController.tutorialScreen;
                    lesson.Initialize(lessonType, screen);
                    TutorialController.lessons.Add(lesson);
                }
            }
            GUI.color = Color.red;
            if (GUILayout.Button("Clear Lessons"))
            {
                int childCount = TutorialController.transform.childCount;
                for (int i = childCount - 1; i >= 0; i--) {
                    GameObject.DestroyImmediate( TutorialController.transform.GetChild( i ).gameObject );
                }
                TutorialController.lessons.Clear();
            }
        }
    }
#endif
}