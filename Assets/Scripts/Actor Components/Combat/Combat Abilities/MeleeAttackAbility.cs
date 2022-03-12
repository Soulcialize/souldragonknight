using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

public class MeleeAttackAbility : CombatAbility
{
    [SerializeField] private bool isReadyRequired;
    [SerializeField] private float readyDuration;
    [SerializeField] private AttackEffectArea attackEffectArea;

    public override void Execute(Combat combat, params object[] parameters)
    {
        if (isReadyRequired)
        {
            combat.CombatStateMachine.ChangeState(new ReadyAttackState(combat, readyDuration, ReadyCallback));
        }
        else
        {
            combat.CombatStateMachine.ChangeState(new MeleeAttackState(combat, attackEffectArea));
        }
    }

    private void ReadyCallback(Combat combat)
    {
        combat.CombatStateMachine.ChangeState(new MeleeAttackState(combat, attackEffectArea));
    }
}
