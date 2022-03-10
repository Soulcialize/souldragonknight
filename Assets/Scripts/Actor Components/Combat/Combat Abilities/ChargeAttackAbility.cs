using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CombatStates;

public class ChargeAttackAbility : CombatAbility
{
    [Tooltip("Distance from the target at which to ready attack.")]
    [SerializeField] private float readyAttackDistance;
    [SerializeField] private float lockTargetPositionTime;
    [SerializeField] private float readyDuration;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeRecoveryTime;

    [Space(10)]

    [SerializeField] private UnityEvent readyChargeStartEvent;
    [SerializeField] private UnityEvent chargeEndEvent;

    public float ReadyAttackDistance { get => readyAttackDistance; }
    public float LockTargetPositionTime { get => lockTargetPositionTime; }
    public float ChargeRecoveryTime { get => chargeRecoveryTime; }

    public override void Execute(Combat combat, params object[] parameters)
    {
        Transform target = (Transform)parameters[0];
        combat.CombatStateMachine.ChangeState(new ReadyChargeAttackState(
            combat, target, lockTargetPositionTime, readyDuration, ReadyCallback, readyChargeStartEvent));
    }

    private void ReadyCallback(Combat combat)
    {
        Vector2 targetPosition = ((ReadyChargeAttackState)combat.CombatStateMachine.CurrState).TargetPosition;
        combat.CombatStateMachine.ChangeState(new ChargeAttackState(
            combat, targetPosition, chargeSpeed, chargeRecoveryTime, chargeEndEvent));
    }
}
