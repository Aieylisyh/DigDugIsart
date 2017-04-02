using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : CharacterAction {

    protected enum PlayerState { Free, Die, ScenarioLock, Attacking, Moving};
    protected PlayerState myPlayerState = PlayerState.Free;

    [SerializeField]
    [Range(0.1f,1)]
    private float attackInterval = 0.3f;
    [SerializeField]
    [Range(0.2f, 1.6f)]
    private float attackRange = 0.5f;

    [SerializeField]
    private GameObject prefab_attack;

    public static PlayerAction instance;

    protected override void Start()
    {
        base.Start();
        if (instance)
            Debug.LogError("multi instance if player found!");
        instance = this;
        TryTurn(Direction.Left);
    }

    protected override void ReceiveControl()
    {
        base.ReceiveControl();
        if (myPlayerState == PlayerState.Die || myPlayerState == PlayerState.ScenarioLock)
            return;
        float hInput = InputManager.instance.horizontalAxis;
        float vInput = InputManager.instance.verticalAxis;
        bool attackInput = InputManager.instance.attackIsOn;
        //attack has piority over move, move horizontally has piority over move vertically
        if (attackInput && (myPlayerState == PlayerState.Free || myPlayerState == PlayerState.Moving))
        {
            isMoving = false;
            Attack();
            myAnimator.speed = 1;
        }
        else if (hInput != 0 && (myPlayerState == PlayerState.Free || myPlayerState == PlayerState.Moving))
        {
            isMoving = true;
            //myPlayerState = PlayerState.Moving;
            TryTurn(hInput>0?Direction.Right:Direction.Left);
            myAnimator.speed = 1;
        }
        else if (vInput != 0 && (myPlayerState == PlayerState.Free || myPlayerState == PlayerState.Moving))
        {
            isMoving = true;
            //myPlayerState = PlayerState.Moving;
            TryTurn(vInput > 0 ? Direction.Up : Direction.Down);
            myAnimator.speed = 1;
        }
        else
        {
            isMoving = false;
            myAnimator.speed = 0;
        }
        myAnimator.SetBool("isDig", Pick.instance.isDigging);
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
        GameObject attackRope = (GameObject)Instantiate(prefab_attack);
        attackRope.GetComponent<AttackRope>().Init(m_direction, shootLength, transform.position, transform.localScale.x );
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
        return DirectionEnumToVector3(m_direction);;
    }

    protected override void Turn(Direction newDirection) {
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
        m_direction = newDirection;
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
                myAnimator.speed = 0.3f;
                break;
            case AnimationState.GoDownHeadingLeft:
                SetAllMoveAnimationParametersFalse();
                myAnimator.SetBool("isWalkdownheadingleft", true);
                break;
            case AnimationState.GoDownHeadingRight:
                SetAllMoveAnimationParametersFalse();
                myAnimator.SetBool("isWalkdownheadingright", true);
                break;
            case AnimationState.GoLeft:
                SetAllMoveAnimationParametersFalse();
                myAnimator.SetBool("isWalkleft", true);
                break;
            case AnimationState.GoRight:
                SetAllMoveAnimationParametersFalse();
                myAnimator.SetBool("isWalkright", true);
                break;
            case AnimationState.GoUpHeadingLeft:
                SetAllMoveAnimationParametersFalse();
                myAnimator.SetBool("isWalkupheadingleft", true);
                break;
            case AnimationState.GoUpHeadingRight:
                SetAllMoveAnimationParametersFalse();
                myAnimator.SetBool("isWalkupheadingright", true);
                break;
            case AnimationState.Idle:
                SetAllMoveAnimationParametersFalse();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && myPlayerState!= PlayerState.Die)
        {
            if (collision.gameObject.GetComponent<EnemyAction>() && collision.gameObject.GetComponent<EnemyAction>().isNotHarmful)
                return;
            //print("hit enemy!");
            myPlayerState = PlayerState.Die;
            isMoving = false;
            SwitchAnimState(AnimationState.Die);
            CancelInvoke();
            GameManager.instance.GameOver();
        }
        if (collision.gameObject.GetComponent<Collectible>())
        {
            Destroy(collision.gameObject);
            GameManager.instance.Score += collision.gameObject.GetComponent<Collectible>().score;
        }
    }

    public void OnWin()
    {
        myPlayerState = PlayerState.ScenarioLock;
        isMoving = false;
        SetAllMoveAnimationParametersFalse();
    }
}
