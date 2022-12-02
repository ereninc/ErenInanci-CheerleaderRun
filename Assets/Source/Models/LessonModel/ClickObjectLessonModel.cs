using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClickObjectLessonModel : LessonModel
{
    public Transform TargetPosition;
    public string HelperText;
    public int ButtonScale;
    public UnityEvent ButtonEvent;
    private RectTransform circleButtonTf;
    private bool LessonLoaded;
    [HideInInspector] public Button CircleButton;
    [HideInInspector] public Image MaskImage;
    [HideInInspector] public Camera MainCamera;
    [HideInInspector] public Canvas Canvas;
    [HideInInspector] public TutorialScreen TutorialScreen;

    public void Initialize(LessonTypes lessonType, TutorialScreen tutorialScreen)
    {
        LessonType = lessonType;
        TutorialScreen = tutorialScreen;
        CircleButton = TutorialScreen.CircleButton;
        MaskImage = TutorialScreen.MaskImage;
        MainCamera = TutorialScreen.MainCamera;
        Canvas = TutorialScreen.Canvas;
    }
    public void Load()
    {
        Debug.Log("Click Object Lesson Load");
        MaskImage.transform.localScale = new Vector3(20, 20, 1);
        MaskImage.transform.DOScale(1, 1f);
        circleButtonTf = CircleButton.GetComponent<RectTransform>();
        if (ButtonScale == 0)
            ButtonScale = 1;
        circleButtonTf.localScale = new Vector3(ButtonScale, ButtonScale, 1);
        circleButtonTf.anchoredPosition = WorldToScreenPoint(MainCamera, Canvas, TargetPosition.position);
        CircleButton.onClick?.RemoveAllListeners();
        CircleButton.onClick?.AddListener(OnCompleteLesson);
        LessonLoaded = true;

    }
    public void OnCompleteLesson()
    {
        Debug.Log("Click Object Lesson End");
        LessonLoaded = false;
        TutorialDataModel.Data.index += 1;
        TutorialDataModel.Data.Save();
        ButtonEvent?.Invoke();
        TutorialScreen.LessonEnd();
    }

    private void Update()
    {
        if (LessonLoaded)
        {
            circleButtonTf.anchoredPosition = WorldToScreenPoint(MainCamera, Canvas, TargetPosition.position);
        }
    }

    private Vector2 WorldToScreenPoint(Camera camera, Canvas canvas, Vector3 targetPos)
    {
        Vector2 myPositionOnScreen = camera.WorldToScreenPoint(targetPos);
        float scaleFactor = canvas.scaleFactor;
        return new Vector2(myPositionOnScreen.x / scaleFactor, myPositionOnScreen.y / scaleFactor);
    }
}
