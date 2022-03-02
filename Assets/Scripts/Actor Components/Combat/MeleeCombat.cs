using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

public class MeleeCombat : Combat
{
    [SerializeField] private AttackEffectArea attackEffectArea;
    [SerializeField] private float minTimeBetweenAttacks;

    public AttackEffectArea AttackEffectArea { get => attackEffectArea; }
    public float MinTimeBetweenAttacks { get => minTimeBetweenAttacks; }

    public override void Attack()
    {
        CombatStateMachine.ChangeState(new MeleeAttackState(this));
    }

    public void StartBlock()
    {
        CombatStateMachine.ChangeState(new BlockState(this));
    }

    public void EndBlock()
    {
        if (CombatStateMachine.CurrState is BlockState blockState)
        {
            CombatStateMachine.Exit();
        }
    }
}
