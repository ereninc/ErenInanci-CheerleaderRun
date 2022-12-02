using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ControllerBaseModel
{
    public float ForwardSpeed;
    public Transform character;
    //[SerializeField] FinishRoad FinishRoad;
    [SerializeField] PointerController pointerController;
    [SerializeField] Vector3 movePos;
    [SerializeField] float sensitve;
    [SerializeField] float xPosition;
    [SerializeField] float lastXPosition;
    [HideInInspector] public int State;
    [SerializeField] float roadLimit;
    float xPos;
    bool isFinished;
    float extraForwardSpeed;

    public override void Initialize()
    {
        base.Initialize();
    }

    public void OnStartGame()
    {
        State = 0;
    }

    public override void ControllerUpdate(GameStates currentState)
    {
        base.ControllerUpdate(currentState);
        if (currentState == GameStates.Game)
        {
            switch (State)
            {
                case 0:
                    movementUpdate();
                    pointerController.ControllerUpdate();
                    break;
                case 1:
                    break;
                default:
                    break;
            }
        }
    }

    public void OnPointerDown()
    {
        lastXPosition = xPosition;
    }

    public void OnPointer()
    {
        if (isFinished == false)
        {
            xPosition = lastXPosition + pointerController.DeltaPosition.x * sensitve;
            xPosition = Mathf.Clamp(xPosition, -roadLimit, roadLimit);
        }
    }

    public void OnPointerUp()
    {
        lastXPosition = xPosition;
    }

    private void movementUpdate()
    {
        xPos = xPosition;
        character.localPosition = Vector3.MoveTowards(character.localPosition, new Vector3(xPos, 0, 0), 0.5f);
        movePos = new Vector3(0, 0, transform.position.z + ((1 + extraForwardSpeed) * ForwardSpeed * Time.deltaTime));
        transform.position = movePos;
    }
}