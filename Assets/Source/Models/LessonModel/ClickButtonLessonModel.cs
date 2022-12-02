using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ClickButtonLessonModel : LessonModel
{
    public Button TargetButton;
    public string HelperText;
    private Button copiedButton;
    [HideInInspector] public Material Mask;
    [HideInInspector] public GameObject HandImageParent;
    [HideInInspector] public TutorialScreen TutorialScreen;
    public void Initialize(LessonTypes lessonType, TutorialScreen tutorialScreen)
    {
        LessonType = lessonType;
        TutorialScreen = tutorialScreen;
        Mask = TutorialScreen.Mask;
        HandImageParent = TutorialScreen.ClickButtonHandParent;
    }
    public void Load(Transform parent)
    {
        Debug.Log("Click Button Lesson Load");
        copiedButton = Instantiate(TargetButton, parent, true);
        copiedButton.transform.localScale = new Vector3(20, 20, 1);
        copiedButton.transform.DOScale(1, 1f);
        copiedButton.image.material = Mask;
        foreach (Transform child in copiedButton.transform) 
            Destroy(child.gameObject);
        copiedButton.onClick = new Button.ButtonClickedEvent();
        copiedButton.onClick = TargetButton.onClick;
        copiedButton.onClick.AddListener(OnCompleteLesson);
        HandImageParent.transform.position = copiedButton.transform.position;
    }

    public void OnCompleteLesson()
    {
        Debug.Log("Click Button Lesson End");
        TutorialDataModel.Data.index += 1;
        TutorialDataModel.Data.Save();
        TutorialScreen.LessonEnd();
        Destroy(copiedButton.gameObject);
    }
}
