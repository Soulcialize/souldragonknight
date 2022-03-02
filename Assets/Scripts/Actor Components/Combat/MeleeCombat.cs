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
}
