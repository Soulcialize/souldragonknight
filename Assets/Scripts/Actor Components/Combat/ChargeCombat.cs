using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CombatStates;

public class ChargeCombat : Combat
{
    [Header("Charge Combat")]

    [Tooltip("Distance from the target at which to ready attack.")]
    [SerializeField] private float readyAttackDistance;
    [SerializeField] private float lockTargetPositionTime;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeRecoveryTime;

    [Space(10)]

    [SerializeField] private UnityEvent chargeEndEvent;

    public float ReadyAttackDistance { get => readyAttackDistance; }
    public float LockTargetPositionTime { get => lockTargetPositionTime; }
    public float ChargeSpeed { get => chargeSpeed; }
    public float ChargeRecoveryTime { get => chargeRecoveryTime; }

    public UnityEvent ChargeEndEvent { get => chargeEndEvent; }

    public override void ReadyAttack(Transform target)
    {
        CombatStateMachine.ChangeState(new ReadyChargeAttackState(this, target));
        ReadyAttackEvent.Invoke();
    }

    public override void Attack()
    {
        Vector2 targetPosition = ((ReadyChargeAttackState)CombatStateMachine.CurrState).TargetPosition;
        CombatStateMachine.ChangeState(new ChargeAttackState(this, targetPosition));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CombatStateMachine.CurrState is ChargeAttackState chargeAttackState)
        {
            chargeAttackState.HandleCollision(collision);
        }
    }
}
