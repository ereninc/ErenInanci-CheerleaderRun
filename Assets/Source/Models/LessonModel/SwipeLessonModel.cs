using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeLessonModel : LessonModel
{
    public string HelperText;
    public List<SwipeDirection> swipeDirections;
    public UnityEvent SwipeEvent;
    [HideInInspector] public TutorialScreen TutorialScreen;
    
    public enum SwipeDirection { None, Left, Up, Right, Down }

    private SwipeDirection direction;
    private Vector2 startPos, endPos;
    private float swipeThreshold = 100f;
    private bool draggingStarted, LessonLoaded;
    
    public void Initialize(LessonTypes lessonType, TutorialScreen tutorialScreen)
    {
        LessonType = lessonType;
        TutorialScreen = tutorialScreen;
    }
    public void Load()
    {
        Debug.Log("Swipe Lesson Load");
        LessonLoaded = true;
    }

    public void OnCompleteLesson()
    {
        LessonLoaded = false;
        Debug.Log("Swipe Lesson End");
        TutorialDataModel.Data.index += 1;
        TutorialDataModel.Data.Save();
        SwipeEvent?.Invoke();
        TutorialScreen.LessonEnd();
    }
    private void Update()
    {
        if (LessonLoaded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                draggingStarted = true;
                startPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                if (draggingStarted)
                {
                    endPos = Input.mousePosition;

                    Vector2 difference = endPos - startPos; // difference vector between start and end positions.

                    if (difference.magnitude > swipeThreshold)
                    {
                        if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y)) // Do horizontal swipe
                        {
                            direction = difference.x > 0 ? SwipeDirection.Right : SwipeDirection.Left; // If greater than zero, then swipe to right.
                        }
                        else //Do vertical swipe
                        {
                            direction = difference.y > 0 ? SwipeDirection.Up : SwipeDirection.Down; // If greater than zero, then swipe to up.
                        }
                    }
                    else
                    {
                        direction = SwipeDirection.None;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                bool swipeMatch = false;
                for (int i = 0; i < swipeDirections.Count; i++)
                {
                    if (swipeDirections[i] == direction)
                    {
                        swipeMatch = true;
                        break;
                    }
                }
                if (draggingStarted && direction != SwipeDirection.None && swipeMatch)
                {
                    //Do something with this direction data.
                    Debug.Log("Swipe direction: " + direction);
                    OnCompleteLesson();
                }

                //reset the variables
                startPos = Vector2.zero;
                endPos = Vector2.zero;
                draggingStarted = false;
            }
        }
       
    }

}
