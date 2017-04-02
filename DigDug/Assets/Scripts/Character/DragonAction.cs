using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAction : EnemyAction
{
    [Header("attack flame")]
    [SerializeField]
    private GameObject prefab_attack;
    [SerializeField]
    private float attackCoolDown = 5;
    private float attackCoolDownRest;
    [SerializeField]
    private float attackDistanceMin = 0.5f;
    [SerializeField]
    private float attackDistanceMax = 5;
    [SerializeField]
    [Range(0.4f, 2)]
    protected float attackPrepareTime = 1.0f;
    [SerializeField]
    [Range(0.4f, 2)]
    protected float attackEndTime = 1.0f;
    [SerializeField]
    [Range(0.05f, 1)]
    private float attackChance = 0.2f;
    [Header("stealth move")]
    [SerializeField]
    private float stealthCoolDown = 5;
    private float stealthCoolDownRest;
    [SerializeField]
    private float stealthMoveDistanceMin = 3;
    [SerializeField]
    private float stealthMoveDistanceMax = 6;
    [SerializeField]
    [Range(0.05f, 1)]
    private float stealthMoveChance = 0.2f;
    protected override void Start()
    {
        base.Start();
        myEnemyType = EnemyType.Dragon;
        stealthCoolDownRest = stealthCoolDown;
        attackCoolDownRest = attackCoolDown;
    }

    protected override void MakeDecision()
    {
        if (stealthCoolDownRest > 0)
            stealthCoolDownRest -= AIThinkInterval;
        if (attackCoolDownRest > 0)
            attackCoolDownRest -= AIThinkInterval;
        switch (myEnemyState)
        {
            case EnemyState.Idle:
                SetState(EnemyState.Moving);
                break;
            case EnemyState.Moving:
                if (stealthCoolDownRest <= 0
                    && GetDistanceToPlayer() > stealthMoveDistanceMin
                    && GetDistanceToPlayer() < stealthMoveDistanceMax 
                    && Random.value < stealthMoveChance)
                {
                    stealthCoolDownRest = stealthCoolDown;
                    SetState(EnemyState.StealthMoving);
                    return;
                }
                if (attackCoolDownRest <= 0
                    && Mathf.Abs(PlayerAction.instance.transform.position.y - this.transform.position.y)< m_turningTolerance*2
                    && GetDistanceToPlayer() > attackDistanceMin
                    && GetDistanceToPlayer() < attackDistanceMax
                    && Random.value < attackChance)
                {
                    //TODO check if player is in front
                    attackCoolDownRest -= AIThinkInterval;
                    SetState(EnemyState.Attacking);
                    return;
                }
                break;
        }
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void ContinueState(EnemyState pState)
    {
        base.ContinueState(pState);
        switch (pState)
        {
            case EnemyState.Moving:
                break;
        }
    }

    protected override void ExitState(EnemyState pState)
    {
        base.ContinueState(pState);
        switch (pState)
        {
            case EnemyState.Attacking:
                myAnimator.SetTrigger("endAttack");
                break;
            case EnemyState.StealthMoving:
                myAnimator.SetTrigger("endStealth");
                isMoving = false;
                MakeChangePositionDecision();
                break;
            case EnemyState.Moving:
                isMoving = false;
                break;
            case EnemyState.Idle:
                break;
        }
        myAnimator.speed = 1;
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }

    protected override void EnterState(EnemyState pState)
    {
        base.ContinueState(pState);
        switch (pState)
        {
            case EnemyState.Idle:
                myAnimator.speed = 0;
                isMoving = false;
                break;
            case EnemyState.Moving:
                isMoving = true;
                myAnimator.speed = 0.5f;
                break;
            case EnemyState.Attacking:
                myAnimator.SetTrigger("startAttack");
                isMoving = false;
                Invoke("Attack", attackPrepareTime);
                break;
            case EnemyState.StealthMoving:
                myAnimator.SetTrigger("startStealth");
                myAnimator.speed = 0.5f;
                print("startStealth");
                isMoving = true;
                targetPosition_StealthMoving = PlayerAction.instance.transform.position;
                float time = GetDistanceToPlayer() / m_moveSpeed;
                Invoke("StopStealth", time);
                break;
            case EnemyState.Die:
                myAnimator.SetTrigger("die");
                myAnimator.speed = 0;
                isMoving = false;
                break;
        }
    }

    private void Attack()
    {
        Invoke("StopAttack", attackEndTime);
    }

    private void StopAttack()
    {
        SetState(EnemyState.Idle);
    }

    private void StopStealth()
    {
        print("StopStealth");
        SetState(EnemyState.Idle);
    }
}
