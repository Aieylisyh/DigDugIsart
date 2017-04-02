using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaAction : EnemyAction
{
    public enum NinjaType { Shuriken, Stealth}
    [SerializeField]
    private NinjaType myNinjaType;
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
    [Range(0.1f, 2)]
    protected float attackPrepareTime = 1.0f;
    [SerializeField]
    [Range(0.1f, 2)]
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
        myEnemyType = EnemyType.Ninja;
        stealthCoolDownRest = stealthCoolDown;
        attackCoolDownRest = attackCoolDown;
        inflatedClipName = "ninja_inflated"; 
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
                if (myNinjaType == NinjaType.Stealth
                    && stealthCoolDownRest <= 0
                    && GetDistanceToPlayer() > stealthMoveDistanceMin
                    && GetDistanceToPlayer() < stealthMoveDistanceMax
                    && Random.value < stealthMoveChance)
                {
                    stealthCoolDownRest = stealthCoolDown;
                    SetState(EnemyState.StealthMoving);
                    return;
                }
                if (myNinjaType == NinjaType.Shuriken
                    && attackCoolDownRest <= 0
                    && Mathf.Abs(PlayerAction.instance.transform.position.y - this.transform.position.y) < 2.2f
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
            case EnemyState.BeingInflated:
                //myAnimator.SetTrigger("endInflat");
                //myAnimator.Play("ninja_walk");
                break;
        }
        myAnimator.speed = 1;
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
                myAnimator.speed = 0.5f;
                Invoke("Attack", attackPrepareTime);
                break;
            case EnemyState.StealthMoving:
                myAnimator.SetTrigger("startStealth");
                myAnimator.speed = 0.5f;
                //print("startStealth");
                isMoving = true;
                targetPosition_StealthMoving = PlayerAction.instance.transform.position;
                float time = GetDistanceToPlayer() / m_moveSpeed;
                Invoke("StopStealth", time);
                break;
            case EnemyState.Die:
                //myAnimator.SetTrigger("die");
                myAnimator.speed = 0;
                isMoving = false;
                break;
            case EnemyState.BeingInflated:
                myAnimator.SetTrigger("startInflat");
                myAnimator.speed = 0;
                isMoving = false;
                break;
        }
    }

    private void Attack()
    {
        GameObject go = (GameObject)Instantiate(prefab_attack, transform.position, Quaternion.identity);
        go.GetComponent<Shuriken>().direction = DirectionEnumToVector3(m_direction);
        Invoke("StopAttack", attackEndTime);
    }

    private void StopAttack()
    {
        SetState(EnemyState.Idle);
    }

    private void StopStealth()
    {
        SetState(EnemyState.Idle);
    }

    protected override void ResumeInflated()
    {
        //myAnimator.SetTrigger("endInflat");
        myAnimator.Play("ninja_walk");
        SetState(EnemyState.Idle);
    }
}
