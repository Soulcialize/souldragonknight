using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using CombatStates;

public class CombatStateMachine : StateMachine
{
    public CombatStateMachine()
    {
        transitions[typeof(ReadyAttackState)] = new HashSet<System.Type>() { typeof(AttackState), typeof(ClashState), typeof(HurtState) };
        transitions[typeof(AttackState)] = new HashSet<System.Type>() { typeof(ClashState), typeof(StunState), typeof(HurtState) };
        transitions[typeof(BlockState)] = new HashSet<System.Type>() { typeof(BlockKnockbackState), typeof(HurtState) };
        transitions[typeof(BlockKnockbackState)] = new HashSet<System.Type>() { typeof(BlockState), typeof(StunState), typeof(HurtState) };
        transitions[typeof(CombatKnockbackState)] = new HashSet<System.Type>() { typeof(StunState), typeof(HurtState) };
        transitions[typeof(StunState)] = new HashSet<System.Type>() { typeof(HurtState) };
        transitions[typeof(HurtState)] = new HashSet<System.Type>() { typeof(HurtState) };
    }
}
