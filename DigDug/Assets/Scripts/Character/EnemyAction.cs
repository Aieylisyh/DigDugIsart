using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : CharacterAction
{
    protected MeshCreator m_world;
    protected enum EnemyState { Idle, Die, StealthMoving, Attacking, Moving, BeingInflated };
    //[SerializeField]//debug only
    protected EnemyState myEnemyState = EnemyState.Idle;
    protected enum EnemyType { Other, Dragon, Ninja };//to be extended
    protected EnemyType myEnemyType = EnemyType.Other;

    [SerializeField]
    [Range(0.5f, 3f)]
    protected float AIThinkInterval = 1f;
    protected float AIThinkTimeRest;
    [SerializeField]
    protected GameObject sprite;
    protected Vector3 targetPosition_StealthMoving;
    private Vector2 lastPosition;

    [Header("inflate")]
    [SerializeField]
    private float lifeRegTime = 0.6f;
    private float lifeRegTimeCount = 0;
    [SerializeField]
    private int life = 4;
    private int lifeCount;

    protected string inflatedClipName;

    [HideInInspector]
    public bool isNotHarmful
    {
        get
        {
            return (myEnemyState == EnemyState.StealthMoving|| myEnemyState == EnemyState.Die);
        }
    }

    protected override void Start()
    {
        base.Start();
        Turn(Direction.Right);
        AIThinkTimeRest = 0;
        lastPosition = GetNextMoveDirectionGrillPosition();
        lifeCount = life;
        lifeRegTimeCount = lifeRegTime;
        m_world = MeshCreator.instance;
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
        if (lifeCount < life)
        {
            lifeRegTimeCount -= Time.fixedDeltaTime;
            if (lifeRegTimeCount < 0)
            {
                lifeCount++;
                lifeRegTimeCount = lifeRegTime;
                if (lifeCount < life)
                {
                    myAnimator.Play(inflatedClipName, 0, 1 - ((float)(lifeCount + 0.5f)) / life);
                }else
                {
                    ResumeInflated();
                }
            }
                
        }
        AIThinkTimeRest -= Time.fixedDeltaTime;
        Vector2 newPosition = GetNextMoveDirectionGrillPosition();
        if (AIThinkTimeRest < 0)
        {
            //make new decision
            AIThinkTimeRest = AIThinkInterval;
            MakeDecision();
        }
        if (myEnemyState!= EnemyState.StealthMoving && newPosition!= lastPosition)
        {
            MakeChangePositionDecision();
        }
        lastPosition = newPosition;
    }

    protected void OnDestroy()
    {
        CancelInvoke();
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
            tempDirection = DirectionEnumToVector3(m_direction);
        }
        return tempDirection;
    }

    protected bool CheckDirtValid(Vector2 offset)
    {
        return m_world.GetBlockType(Mathf.RoundToInt(transform.position.x - m_gap + offset.x), Mathf.RoundToInt(transform.position.y - m_gap + offset.y)) == MeshCreator.MAP_TYPE.EMPTY;
    }

    protected Vector2 GetNextMoveDirectionGrillPosition()
    {
        Vector2 offset = GetCurrentDirectionVector() * 0.6f;
        Vector2 position = new Vector2(transform.position.x - m_gap, transform.position.y - m_gap)+ offset;
        return new Vector2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
    }

    protected void MakeChangePositionDecision()
    {
        float testLength = 0.6f;
        bool upValid = CheckDirtValid(Vector3.up * testLength);
        bool leftValid = CheckDirtValid(Vector3.left * testLength);
        bool downValid = CheckDirtValid(Vector3.down * testLength);
        bool rightValid = CheckDirtValid(Vector3.right * testLength);
        if(!rightValid && !downValid && !leftValid && !upValid)
        {
            print("!!! blockedall the way");
            TryKillSelf();
            return;
        }
        if (CheckDirtValid(GetCurrentDirectionVector() * testLength) && CheckDirtValid(-GetCurrentDirectionVector() * testLength))
        {
            //print("continue direction");
            return;
        }
        float deltaX = PlayerAction.instance.transform.position.x - transform.position.x;
        float deltaY = PlayerAction.instance.transform.position.y - transform.position.y;
        if(Mathf.Abs(deltaX)> Mathf.Abs(deltaY))
        {
            if (deltaX > 0 && rightValid)
            {
                TryTurn(Direction.Right);
                return;
            }
            else if(leftValid)
            {
                TryTurn(Direction.Left);
                return;
            }
        }
        else
        {
            if (deltaY > 0 && upValid)
            {
                TryTurn(Direction.Up);
                return;
            }
            else if (downValid)
            {
                TryTurn(Direction.Down);
                return;
            }
        }
        if(!CheckDirtValid(GetCurrentDirectionVector() * testLength))
        {
            if (Random.value < 0.5f)
            {
                if (upValid)
                    TryTurn(Direction.Up);
                else if (downValid)
                    TryTurn(Direction.Down);
                else if (rightValid)
                    TryTurn(Direction.Right);
                else if (leftValid)
                    TryTurn(Direction.Left);
            }
            else
            {
                if (rightValid)
                    TryTurn(Direction.Right);
                else if (leftValid)
                    TryTurn(Direction.Left);
                else if (upValid)
                    TryTurn(Direction.Up);
                else if (downValid)
                    TryTurn(Direction.Down);
            }
        }
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

    protected override void SwitchAnimState(AnimationState newAnimationState)
    {
        base.SwitchAnimState(newAnimationState);
        switch (newAnimationState)
        {
            case AnimationState.GoLeft:
                sprite.transform.localScale = new Vector3(-1, 1, 1);
                break;
            case AnimationState.GoRight:
                sprite.transform.localScale = Vector3.one;
                //scale
                break;
        }
    }

    protected virtual void TryKillSelf()
    {
        SetState(EnemyState.Die);
        Destroy(gameObject, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            OnInflated();
        }
    }

    protected virtual void OnInflated()
    {
        lifeCount--;
        CancelInvoke();
        if (lifeCount < 0)
        {
            TryKillSelf();
        }
        else
        {
            SetState(EnemyState.BeingInflated);
            lifeRegTimeCount = lifeRegTime;
            myAnimator.Play(inflatedClipName, 0, 1 - ((float)(lifeCount + 0.5f)) / life);
        }
    }

    protected virtual void ResumeInflated()
    {
    }
}
