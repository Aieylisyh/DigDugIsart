﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : CharacterAction {

    protected enum PlayerState { Free, Die, ScenarioLock, Attacking, Moving};
    protected PlayerState myPlayerState = PlayerState.Free;

    [SerializeField]
    [Range(0.1f,1)]
    private float attackInterval = 0.5f;
    [SerializeField]//enable this for debugging
    private bool isDigging = false;
    [SerializeField]
    [Range(1f, 4f)]
    private float attackRange = 1.0f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void ReceiveControl()
    {
        base.ReceiveControl();
        float hInput = InputManager.instance.horizontalAxis;
        float vInput = InputManager.instance.verticalAxis;
        bool attackInput = InputManager.instance.attackIsOn;
        //attack has piority over move, move horizontally has piority over move vertically
        if (attackInput && (myPlayerState == PlayerState.Free || myPlayerState == PlayerState.Moving))
        {
            isMoving = false;
            Attack();
        }else if (hInput != 0 && (myPlayerState == PlayerState.Free || myPlayerState == PlayerState.Moving))
        {
            isMoving = true;
            //myPlayerState = PlayerState.Moving;
            TryTurn(hInput>0?Direction.Right:Direction.Left);
        }else if (vInput != 0 && (myPlayerState == PlayerState.Free || myPlayerState == PlayerState.Moving))
        {
            isMoving = true;
            //myPlayerState = PlayerState.Moving;
            TryTurn(vInput > 0 ? Direction.Up : Direction.Down);
        }
        else
        {
            isMoving = false;
        }
    }

    private void Attack()
    {
        print("player Attack!");
        myPlayerState = PlayerState.Attacking;
        float shootLength = attackRange;//get attack length
        bool hitEnemy = false;//get enemy hit
        if (hitEnemy)
        {
            SwitchAnimState(AnimationState.Charge);
        }
        else
        {
            SwitchAnimState(AnimationState.Attack);
        }
        //Instantiate();
        //show rope
        Invoke("AttackEnd", attackInterval);
    }

    private void AttackEnd()
    {
        print("AttackEnd");
        myPlayerState = PlayerState.Free;
        SwitchAnimState(GetCurrentIdleAnimationState());
    }

    protected override Vector3 GetCurrentDirectionVector()
    {
        Vector3 tempDirection = Vector3.left;
        switch (myDirection)
        {
            case Direction.Down:
                tempDirection = Vector3.down;
                break;
            case Direction.Up:
                tempDirection = Vector3.up;
                break;
            case Direction.Right:
                tempDirection = Vector3.right;
                break;
            case Direction.Left:
                tempDirection = Vector3.left;
                break;
        }
        return tempDirection;
    }

    private void TryTurn(Direction newDirection)
    {
        //TODO: must turn in a turning point
        //if digging current block is not finished, continue digging, else turn
        Turn(newDirection);
    }

    protected override void Turn(Direction newDirection)
    {
        base.Turn(newDirection);
        switch (newDirection)
        {
            case Direction.Down:
                if (myHorizontalFacing == HorizontalFacing.Left)
                {
                    SwitchAnimState(AnimationState.GoDownHeadingLeft);
                }
                else{
                    SwitchAnimState(AnimationState.GoDownHeadingRight);
                }
                break;
            case Direction.Up:
                if (myHorizontalFacing == HorizontalFacing.Left)
                {
                    SwitchAnimState(AnimationState.GoUpHeadingLeft);
                }
                else
                {
                    SwitchAnimState(AnimationState.GoUpHeadingRight);
                }
                break;
            case Direction.Left:
                SwitchAnimState(AnimationState.GoLeft);
                myHorizontalFacing = HorizontalFacing.Left;
                break;
            case Direction.Right:
                SwitchAnimState(AnimationState.GoRight);
                myHorizontalFacing = HorizontalFacing.Right;
                break;
        }
        myDirection = newDirection;
    }

    protected override void SwitchAnimState(AnimationState newAnimationState)
    {
        base.SwitchAnimState(newAnimationState);
        switch (newAnimationState)
        {
            case AnimationState.Attack:
                myAnimator.SetTrigger("attack");
                break;
            case AnimationState.Charge:
                myAnimator.SetTrigger("charge");
                break;
            case AnimationState.Die:
                myAnimator.SetTrigger("die");
                break;
            case AnimationState.GoDownHeadingLeft:
                SetAllMoveAnimationParametersFalse();
                myAnimator.SetBool("isDig", isDigging);
                myAnimator.SetBool("isWalkdownheadingleft", true);
                break;
            case AnimationState.GoDownHeadingRight:
                SetAllMoveAnimationParametersFalse();
                myAnimator.SetBool("isDig", isDigging);
                myAnimator.SetBool("isWalkdownheadingright", true);
                break;
            case AnimationState.GoLeft:
                SetAllMoveAnimationParametersFalse();
                myAnimator.SetBool("isDig", isDigging);
                myAnimator.SetBool("isWalkleft", true);
                break;
            case AnimationState.GoRight:
                SetAllMoveAnimationParametersFalse();
                myAnimator.SetBool("isDig", isDigging);
                myAnimator.SetBool("isWalkright", true);
                break;
            case AnimationState.GoUpHeadingLeft:
                SetAllMoveAnimationParametersFalse();
                myAnimator.SetBool("isDig", isDigging);
                myAnimator.SetBool("isWalkupheadingleft", true);
                break;
            case AnimationState.GoUpHeadingRight:
                SetAllMoveAnimationParametersFalse();
                myAnimator.SetBool("isDig", isDigging);
                myAnimator.SetBool("isWalkupheadingright", true);
                break;
            case AnimationState.Idle:
                SetAllMoveAnimationParametersFalse();
                myAnimator.SetBool("isDig", false);
                break;
        }
    }

    private void SetAllMoveAnimationParametersFalse()
    {
        myAnimator.SetBool("isWalkdownheadingleft", false);
        myAnimator.SetBool("isWalkdownheadingright", false);
        myAnimator.SetBool("isWalkupheadingleft", false);
        myAnimator.SetBool("isWalkupheadingright", false);
        myAnimator.SetBool("isWalkleft", false);
        myAnimator.SetBool("isWalkright", false);
    }
}