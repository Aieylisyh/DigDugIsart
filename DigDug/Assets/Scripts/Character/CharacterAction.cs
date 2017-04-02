using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    protected Animator myAnimator;

    public enum Direction { Up, Down, Right, Left, Other };
    protected Direction myDirection = Direction.Left;
    protected enum HorizontalFacing { Right, Left };
    protected HorizontalFacing myHorizontalFacing = HorizontalFacing.Left;
    protected enum AnimationState { GoUpHeadingRight, GoDownHeadingRight, GoUpHeadingLeft, GoDownHeadingLeft, GoRight, GoLeft, Die, Attack, Charge, Stealth, UnStealth, BecomeFlat, Idle};

    [SerializeField]
    protected float moveSpeed = 3;
    protected bool isMoving = false;
    // Use this for initialization
    protected virtual void Start()
    {
        InitAnim();
    }

    protected virtual void InitAnim()
    {
        myAnimator = GetComponentInChildren<Animator>();
        if (!myAnimator)
            Debug.LogError("myAnimator is not set!");
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        ReceiveControl();
        if(isMoving)
            Move();
    }

    protected virtual void Move()
    {
        Vector3 speed = moveSpeed * Time.fixedDeltaTime * GetCurrentDirectionVector();
        transform.position = transform.position + speed;
    }

    protected virtual void Turn(Direction newDirection)
    {
        //print("Turn to " + newDirection);
    }

    protected virtual void SwitchAnimState(AnimationState newAnimationState)
    {
        //print("SwitchAnimState to "+ newAnimationState);
    }

    protected virtual Vector3 GetCurrentDirectionVector()
    {
        //must be override
        return Vector3.left;
    }

    protected virtual void ReceiveControl()
    {
        //must be override, Receive Control from AI or player input
    }

    protected AnimationState GetCurrentIdleAnimationState()
    {
        AnimationState tempAnimationState = AnimationState.Idle;
        switch (myDirection)
        {
            case Direction.Down:
                if (myHorizontalFacing == HorizontalFacing.Left)
                {
                    tempAnimationState = AnimationState.GoDownHeadingLeft;
                }
                else
                {
                    tempAnimationState = AnimationState.GoDownHeadingRight;
                }
                break;
            case Direction.Up:
                if (myHorizontalFacing == HorizontalFacing.Left)
                {
                    tempAnimationState = AnimationState.GoUpHeadingLeft;
                }
                else
                {
                    tempAnimationState = AnimationState.GoUpHeadingRight;
                }
                break;
            case Direction.Left:
                tempAnimationState = AnimationState.GoLeft;
                break;
            case Direction.Right:
                tempAnimationState = AnimationState.GoRight;
                break;
        }
        return tempAnimationState;
    }
}
