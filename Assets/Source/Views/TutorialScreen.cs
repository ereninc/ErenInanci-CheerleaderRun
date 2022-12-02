using System.Collections;
using System.Collections.Generic;
using CubeGames.Tutorial;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreen : ScreenElement
{
    public GameObject Swipe;
    public Animator swipeAnimator;
    public GameObject TapAndHold;
    public GameObject ClickObject;
    public Image Background;
    [Space(10)] public Text SwipeText;
    [Space(10)] public Button CircleButton;
    public Image MaskImage;
    public Camera MainCamera;
    public Canvas Canvas;
    public Text ClickObjectText;
    [Space(10)] public Material Mask;
    public GameObject ClickButtonHandParent;
    public Text ClickButtonText;
    [Space(10)] public Button TapHoldButton;
    public Text TapHoldText;

    public void SetActiveLesson(LessonTypes lessonType)
    {
        switch (lessonType)
        {
            case LessonTypes.Swipe:
                {
                    Swipe.SetActive(true);
                    var swipe = TutorialController.Instance.lessons[TutorialDataModel.Data.index]
                      .GetComponent<SwipeLessonModel>();
                    SwipeDirectionCheck(swipe);
                    SwipeText.text = swipe.HelperText;
                    Background.SetActiveGameObject(true);
                    break;
                }
            case LessonTypes.TapHold:
                {
                    var tapHold = TutorialController.Instance.lessons[TutorialDataModel.Data.index]
                      .GetComponent<TapHoldLessonModel>();
                    TapHoldText.text = tapHold.HelperText;
                    TapAndHold.SetActive(true);
                    Background.SetActiveGameObject(true);
                    break;
                }
            case LessonTypes.ClickButton:
                {
                    var clickButton = TutorialController.Instance.lessons[TutorialDataModel.Data.index]
                      .GetComponent<ClickButtonLessonModel>();
                    ClickButtonText.text = clickButton.HelperText;
                    Background.SetActiveGameObject(true);
                    ClickButtonHandParent.SetActive(true);
                    break;
                }
            case LessonTypes.ClickObject:
                {
                    var clickObject = TutorialController.Instance.lessons[TutorialDataModel.Data.index]
                      .GetComponent<ClickObjectLessonModel>();
                    ClickObjectText.text = clickObject.HelperText;
                    ClickObject.SetActive(true);
                    Background.SetActiveGameObject(true);
                    break;
                }
        }
    }

    public void LessonEnd()
    {
        Swipe.SetActive(false);
        ClickObject.SetActive(false);
        TapAndHold.SetActive(false);
        Background.SetActiveGameObject(false);
        ClickButtonHandParent.SetActive(false);
    }

    private void SwipeDirectionCheck(SwipeLessonModel swipe)
    {
        if (swipe.swipeDirections.Count == 1)
        {
            switch (swipe.swipeDirections[0])
            {
                case SwipeLessonModel.SwipeDirection.Right:
                    swipeAnimator.Play("Right");
                    break;
                case SwipeLessonModel.SwipeDirection.Left:
                    swipeAnimator.Play("Left");
                    break;
                case SwipeLessonModel.SwipeDirection.Up:
                    swipeAnimator.Play("Up");
                    break;
                case SwipeLessonModel.SwipeDirection.Down:
                    swipeAnimator.Play("Down");
                    break;
            }
        }
        else if (swipe.swipeDirections.Count > 1)
        {
            if (swipe.swipeDirections[0] == SwipeLessonModel.SwipeDirection.Right || swipe.swipeDirections[0] == SwipeLessonModel.SwipeDirection.Left && swipe.swipeDirections[1] == SwipeLessonModel.SwipeDirection.Right || swipe.swipeDirections[1] == SwipeLessonModel.SwipeDirection.Left)
            {
                swipeAnimator.Play("RightLeft");
            }
            if (swipe.swipeDirections[0] == SwipeLessonModel.SwipeDirection.Up || swipe.swipeDirections[0] == SwipeLessonModel.SwipeDirection.Down && swipe.swipeDirections[1] == SwipeLessonModel.SwipeDirection.Up || swipe.swipeDirections[1] == SwipeLessonModel.SwipeDirection.Down)
            {
                swipeAnimator.Play("UpDown");
            }
        }
    }
 
}
