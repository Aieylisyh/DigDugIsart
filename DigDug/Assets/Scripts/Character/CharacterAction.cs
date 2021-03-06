﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    protected Animator myAnimator;

    public enum Direction { Up = 1, Down = 3, Right = 0, Left = 4, Other = 5 };
    protected Direction m_direction = Direction.Left;
    protected Direction m_nextDirection;
    protected enum HorizontalFacing { Right, Left };
    protected HorizontalFacing myHorizontalFacing = HorizontalFacing.Left;
    protected enum AnimationState { GoUpHeadingRight, GoDownHeadingRight, GoUpHeadingLeft, GoDownHeadingLeft, GoRight, GoLeft, Die, Attack, Charge, Stealth, UnStealth, BecomeFlat, Idle};

    [SerializeField]
    protected float m_moveSpeed = 0.7f;
    protected bool isMoving = false;

    [SerializeField]
    protected float m_turningTolerance = 0.1f;
    protected float m_gap;

    protected virtual void Awake () {
        m_moveSpeed *= transform.localScale.x;
        m_gap = transform.localPosition.x % 1;
    }

    protected virtual void Start()
    {
        InitAnim();
        StartCoroutine( TurningProcess() );
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
        Vector3 speed = m_moveSpeed * Time.fixedDeltaTime * GetCurrentDirectionVector();
        transform.position = transform.position + speed;
    }

    protected virtual void TryTurn(Direction newDirection)
    {
        m_nextDirection = newDirection;
    }

    protected virtual void Turn(Direction newDirection)
    {
        //print("Turn to " + newDirection);
    }

    private IEnumerator TurningProcess () {

        while (true) {

            if (m_nextDirection != m_direction) {

                if ((byte)m_nextDirection + (byte)m_direction == 4) {
                    Turn(m_nextDirection);
                } else {
                    switch (m_nextDirection) {
                        case Direction.Up:
                        case Direction.Down:
                            if (OnGrid(transform.localPosition.x)) {
                                Turn( m_nextDirection );
                            }
                            break;
                        default:
                            if (OnGrid( transform.localPosition.y )) {
                                Turn( m_nextDirection );
                            }
                            break;
                    }
                }
            }

            yield return null;
        }
    }

    private bool OnGrid (Vector3 pos) {
        return OnGrid( pos.x ) && OnGrid( pos.y );
    }

    private bool OnGrid(float axe) {
        axe -= m_gap;
        float dif = Mathf.Abs( axe - Mathf.Round(axe) );
        return dif < m_turningTolerance;
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
        switch (m_direction)
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

    protected Direction GetInversedDirection(Direction dir)
    {
        if (dir == Direction.Other)
            return Direction.Other;
        return (Direction)(4 - (byte)dir);
    }
    protected Direction GetTurn90DegreeDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Down:
                return Direction.Right;
            case Direction.Right:
                return Direction.Up;
            case Direction.Up:
                return Direction.Left;
            case Direction.Left:
                return Direction.Down;
            case Direction.Other:
                return Direction.Other;
        }
        return Direction.Other;
    }

    protected Vector3 DirectionEnumToVector3(Direction dir)
    {
        Vector3 tempDirection = Vector3.zero;
        switch (dir)
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
            case Direction.Other:
                tempDirection = Vector3.zero;
                break;
        }
        return tempDirection;
    }
}
