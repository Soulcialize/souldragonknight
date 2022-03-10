using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using CombatStates;

public class CombatStateMachine : StateMachine
{
    public CombatStateMachine()
    {
        transitions[typeof(ReadyAttackState)] = new HashSet<System.Type>() { typeof(AttackState), typeof(ClashState), typeof(HurtState), typeof(DeathState) };
        transitions[typeof(AttackState)] = new HashSet<System.Type>() { typeof(ClashState), typeof(StunState), typeof(HurtState), typeof(DeathState) };
        transitions[typeof(DodgeState)] = new HashSet<System.Type>() { typeof(HurtState), typeof(DeathState) };
        transitions[typeof(BlockState)] = new HashSet<System.Type>() { typeof(BlockKnockbackState), typeof(HurtState), typeof(DeathState) };
        transitions[typeof(BlockKnockbackState)] = new HashSet<System.Type>() { typeof(BlockState), typeof(StunState), typeof(HurtState), typeof(DeathState) };
        transitions[typeof(CombatKnockbackState)] = new HashSet<System.Type>() { typeof(StunState), typeof(HurtState), typeof(DeathState) };
        transitions[typeof(StunState)] = new HashSet<System.Type>() { typeof(HurtState), typeof(DeathState) };
        transitions[typeof(HurtState)] = new HashSet<System.Type>() { typeof(HurtState), typeof(DeathState) };
        transitions[typeof(DeathState)] = new HashSet<System.Type>();
    }
}
