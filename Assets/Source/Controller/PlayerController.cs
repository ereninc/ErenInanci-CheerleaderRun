using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public PlayerMoveTypes MoveType;
    public float ForwardSpeed;
    public float RotationSpeed;
    public float ExtraForwardSpeed;

    private void movementUpdate()
    {
        switch (MoveType)
        {
            case PlayerMoveTypes.FollowPath:
                followPathMovementUpdate();
                break;
            case PlayerMoveTypes.OnlyForward:
                onlyForwardMovementUpdate();
                break;
        }
    }

    private void followPathMovementUpdate()
    {

    }

    private void onlyForwardMovementUpdate()
    {

    }
}