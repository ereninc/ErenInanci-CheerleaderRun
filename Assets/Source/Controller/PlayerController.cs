using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ControllerBaseModel
{
    [SerializeField] private float forwardSpeed;
    [HideInInspector] public int State;
    [SerializeField] private PointerController pointerController;
    [SerializeField] private CharacterModel characterModel;
    [SerializeField] private Vector3 movePos;
    [SerializeField] private float sensitive;
    [SerializeField] float roadLimit;
    //[SerializeField] FinishRoad FinishRoad;
    private float xPosition;
    private float lastXPosition;
    private float xPos;
    private bool isFinished;
    private float extraForwardSpeed;

    public override void Initialize()
    {
        base.Initialize();
        characterModel.Initialize();
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
            xPosition = lastXPosition + pointerController.DeltaPosition.x * sensitive;
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
        characterModel.transform.localPosition = Vector3.MoveTowards(characterModel.transform.localPosition, new Vector3(xPos, 0, 0), 0.5f);
        movePos = new Vector3(0, 0, transform.position.z + ((1 + extraForwardSpeed) * forwardSpeed * Time.deltaTime));
        transform.position = movePos;
    }
}