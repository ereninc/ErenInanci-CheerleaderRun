using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TapHoldLessonModel : LessonModel
{
    public string HelperText;
    public UnityEvent TapAndHoldEvent;
    [HideInInspector] public Button TapAndHoldButton;
    [HideInInspector] public TutorialScreen TutorialScreen;

    public void Initialize(LessonTypes lessonType, TutorialScreen tutorialScreen)
    {
        LessonType = lessonType;
        TutorialScreen = tutorialScreen;
        TapAndHoldButton = TutorialScreen.TapHoldButton;
    }
    public void Load()
    {
        Debug.Log("Tap & Hold Lesson Load");
        TapAndHoldButton.onClick?.RemoveAllListeners();
        TapAndHoldButton.onClick?.AddListener(OnCompleteLesson);
    }

    public  void OnCompleteLesson()
    {
        Debug.Log("Tap & Hold  Lesson End");
        TutorialDataModel.Data.index += 1;
        TutorialDataModel.Data.Save();
        TapAndHoldEvent?.Invoke();
        TutorialScreen.LessonEnd();
    }
}
