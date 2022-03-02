using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

public class ChargeCombat : Combat
{
    [Tooltip("Distance from the target at which to ready attack.")]
    [SerializeField] private float readyAttackDistance;
    [SerializeField] private float chargeSpeed;

    public float ReadyAttackDistance { get => readyAttackDistance; }
    public float ChargeSpeed { get => chargeSpeed; }


    public override void ReadyAttack(Transform target)
    {
        CombatStateMachine.ChangeState(new ReadyChargeAttackState(this, target));
    }

    public override void Attack()
    {
        Vector2 targetPosition = ((ReadyChargeAttackState)CombatStateMachine.CurrState).TargetPosition;
        CombatStateMachine.ChangeState(new ChargeAttackState(this, targetPosition));
    }

    public void OnLockTargetPosition()
    {
        if (CombatStateMachine.CurrState is ReadyChargeAttackState readyChargeAttackState)
        {
            readyChargeAttackState.LockTargetPosition();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CombatStateMachine.CurrState is ChargeAttackState chargeAttackState)
        {
            chargeAttackState.HandleCollision(collision);
        }
    }
}
