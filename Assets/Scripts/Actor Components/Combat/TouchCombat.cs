using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

public class TouchCombat : Combat
{
    [Tooltip("Distance from the target at which to ready attack.")]
    [SerializeField] private float readyAttackDistance;
    [SerializeField] private float chargeSpeed;

    public float ReadyAttackDistance { get => readyAttackDistance; }
    public float ChargeSpeed { get => chargeSpeed; }

    public override void ReadyAttack(Transform target)
    {
        CombatStateMachine.ChangeState(new ReadyTouchAttackState(this, target));
    }

    public override void Attack()
    {
        Vector2 targetPosition = ((ReadyTouchAttackState)CombatStateMachine.CurrState).TargetPosition;
        CombatStateMachine.ChangeState(new TouchAttackState(this, targetPosition));
    }

    public void OnLockTargetPosition()
    {
        if (CombatStateMachine.CurrState is ReadyTouchAttackState readyTouchAttackState)
        {
            readyTouchAttackState.LockTargetPosition();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CombatStateMachine.CurrState is TouchAttackState touchAttackState)
        {
            touchAttackState.HandleCollision(collision);
        }
    }
}
