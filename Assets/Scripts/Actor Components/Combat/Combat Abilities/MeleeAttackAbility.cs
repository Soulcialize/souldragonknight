using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

public class MeleeAttackAbility : CombatAbility
{
    [SerializeField] private bool isReadyRequired;
    [SerializeField] private float readyDuration;
    [SerializeField] private AttackEffectArea attackEffectArea;

    public AttackEffectArea AttackEffectArea { get => attackEffectArea; }

    public float MaximumReach { get => Vector2.Distance(transform.localPosition, attackEffectArea.TopCornerPos); }

    public override void Execute(Combat combat, params object[] parameters)
    {
        if (combat.Resource.CanConsume(resourceCost))
        {
            if (isReadyRequired)
            {
                combat.ActionStateMachine.ChangeState(new ReadyAttackState(combat, readyDuration, ReadyCallback));
            }
            else
            {
                combat.ActionStateMachine.ChangeState(new MeleeAttackState(combat, attackEffectArea, resourceCost));
            }
        }
    }

    private void ReadyCallback(Combat combat)
    {
        combat.ActionStateMachine.ChangeState(new MeleeAttackState(combat, attackEffectArea, resourceCost));
    }
}
