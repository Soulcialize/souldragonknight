using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

public class MeleeCombat : Combat
{
    [SerializeField] private AttackEffectArea attackEffectArea;
    [SerializeField] private float minTimeBetweenAttacks;
    [SerializeField] private float blockKnockbackSpeed;
    [SerializeField] private float blockKnockbackDistance;

    public AttackEffectArea AttackEffectArea { get => attackEffectArea; }
    public float MinTimeBetweenAttacks { get => minTimeBetweenAttacks; }
    public float BlockKnockbackSpeed { get => blockKnockbackSpeed; }
    public float BlockKnockbackDistance { get => blockKnockbackDistance; }

    public override void Attack()
    {
        if (CombatStateMachine.CurrState is ReadyAttackState readyAttackState)
        {
            CombatStateMachine.ChangeState(new MeleeAttackState(this, readyAttackState.StartTime));
        }
        else
        {
            CombatStateMachine.ChangeState(new MeleeAttackState(this, Time.time));
        }
    }

    public void StartBlock()
    {
        CombatStateMachine.ChangeState(new BlockState(this));
    }

    public void EndBlock()
    {
        if (CombatStateMachine.CurrState is BlockState)
        {
            CombatStateMachine.Exit();
        }
    }

    public void KnockbackDuringBlock(Vector2 direction)
    {
        CombatStateMachine.ChangeState(new BlockKnockbackState(
            this, direction, ((BlockState)CombatStateMachine.CurrState).StartTime));
    }
}
