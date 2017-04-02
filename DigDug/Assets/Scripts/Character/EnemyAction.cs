using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : CharacterAction
{
    [SerializeField]
    protected MeshCreator m_world;
    protected enum EnemyState { Idle, Die, StealthMoving, Attacking, Moving, BeingInflated };
    [SerializeField]//debug only
    protected EnemyState myEnemyState = EnemyState.Idle;
    protected enum EnemyType { Other, Dragon, Ninja };//to be extended
    protected EnemyType myEnemyType = EnemyType.Other;
    [SerializeField]
    [Range(0.4f, 2)]
    private float attackPrepareTime = 1.0f;
    [SerializeField]
    [Range(0.4f, 2)]
    private float attackEndTime = 1.0f;

    [SerializeField]
    [Range(0.5f, 3f)]
    private float AIThinkInterval = 1f;
    private float AIThinkTimeRest;
    [SerializeField]
    protected GameObject sprite;
    private Vector3 targetPosition_StealthMoving;
    private Vector2 lastPosition;
    protected override void Start()
    {
        base.Start();
        Turn(Direction.Right);
        AIThinkTimeRest = 0;
        lastPosition = GetNextMoveDirectionGrillPosition();
    }

    protected float GetDistanceToPlayer()
    {
        Vector3 distance = PlayerAction.instance.transform.position - this.transform.position;
        distance.z = 0;
        return distance.magnitude;
    }

    protected override void ReceiveControl()
    {
        base.ReceiveControl();
        AIThinkTimeRest -= Time.fixedDeltaTime;
        Vector2 newPosition = GetNextMoveDirectionGrillPosition();
        if (AIThinkTimeRest < 0)
        {
            //make new decision
            AIThinkTimeRest = AIThinkInterval;
            MakeDecision();
        }else if (myEnemyState!= EnemyState.StealthMoving && newPosition!= lastPosition)
        {
            lastPosition = newPosition;
            MakeDecision();
        }
    }

    protected virtual void MakeDecision()
    {
    }

    protected void SetState(EnemyState newState)
    {
        if(myEnemyState == newState)
        {
            ContinueState(newState);
        }else
        {
            ExitState(myEnemyState);
            EnterState(newState);
            myEnemyState = newState;
        }
    }

    protected virtual void ContinueState(EnemyState pState)
    {
        //print("ContinueState " + pState);
        //TODO override
    }

    protected virtual void ExitState(EnemyState pState)
    {
        //print("ExitState " + pState);
        //TODO override
    }

    protected virtual void EnterState(EnemyState pState)
    {
        //print("EnterState " + pState);
        //TODO override
    }

    protected override Vector3 GetCurrentDirectionVector()
    {
        Vector3 tempDirection = Vector3.left;
        if (myEnemyState== EnemyState.StealthMoving)
        {
            tempDirection = (targetPosition_StealthMoving - transform.position).normalized;
        }else
        {
            switch (m_direction)
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
        }
        return tempDirection;
    }

    protected bool CheckDirtValid(Vector2 offset)
    {
        return m_world.GetBlockType(Mathf.RoundToInt(transform.position.x - m_gap + offset.x), Mathf.RoundToInt(transform.position.y - m_gap + offset.y)) == MeshCreator.MAP_TYPE.EMPTY;
    }

    protected Vector2 GetNextMoveDirectionGrillPosition()
    {
        Vector2 offset = GetCurrentDirectionVector() * 0.5f;
        Vector2 position = new Vector2(transform.position.x - m_gap, transform.position.y - m_gap)+ offset;
        return new Vector2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
    }

    protected override void Turn(Direction newDirection)
    {
        base.Turn(newDirection);
        switch (newDirection)
        {
            case Direction.Down:
                //in Digdug orginal game, enemies can not turn to down or up
                //but we can add this feature if it is good, currently no
                break;
            case Direction.Up:
                //in Digdug orginal game, enemies can not turn to down or up
                //but we can add this feature if it is good, currently no
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
        m_direction = newDirection;
    }
}
