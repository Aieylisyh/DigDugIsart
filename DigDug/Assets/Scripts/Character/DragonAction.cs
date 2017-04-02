using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAction : EnemyAction
{
    [SerializeField]
    private GameObject prefab_attack;

    protected override void Start()
    {
        base.Start();
        myEnemyType = EnemyType.Dragon;
    }

    protected override void MakeDecision()
    {
        switch (myEnemyState)
        {
            case EnemyState.Idle:
                SetState(EnemyState.Moving);
                break;
            case EnemyState.Moving:
                if (!CheckDirtValidToGoTo(m_direction)){
                    print("!");
                    TryTurn(GetInversedDirection(m_direction));
                }
                SetState(EnemyState.Moving);
                break;
        }
    }

    protected override void Move()
    {
        base.Move();
    }

    private void AttackStart()
    {
        print("Dragon AttackStart!");
        myEnemyState = EnemyState.Attacking;
    }

    private void Attack()
    {
        print("Dragon Attack!");
    }

    private void AttackEnd()
    {
        print("Dragon AttackEnd");
        myEnemyState = EnemyState.Idle;
    }

    protected override void SwitchAnimState(AnimationState newAnimationState)
    {
        print("enemy SwitchAnimState to " + newAnimationState);
        base.SwitchAnimState(newAnimationState);
        switch (newAnimationState)
        {
            case AnimationState.Attack:
                myAnimator.SetTrigger("startAttack");
                isMoving = false;
                break;
            case AnimationState.Die:
                isMoving = false;
                break;
            case AnimationState.GoLeft:
                sprite.transform.localScale = new Vector3(-1, 1, 1);
                myAnimator.speed = 0.6f;
                isMoving = true;
                break;
            case AnimationState.GoRight:
                sprite.transform.localScale = Vector3.one;
                myAnimator.speed = 0.6f;
                isMoving = true;
                //scale
                break;
            case AnimationState.Idle:
                //myAnimator.SetBool("isWalk", false);
                myAnimator.speed = 0;
                isMoving = false;
                break;
        }
    }
}
