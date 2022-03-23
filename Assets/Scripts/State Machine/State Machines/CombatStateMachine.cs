using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using CombatStates;

public class CombatStateMachine : StateMachine
{
    public CombatStateMachine()
    {
        transitions[typeof(ReadyAttackState)] = new HashSet<System.Type>() { typeof(AttackState), typeof(BlockState), typeof(HurtState), typeof(DeathState) };
        transitions[typeof(AttackState)] = new HashSet<System.Type>() { typeof(BlockState), typeof(StunState), typeof(HurtState), typeof(DeathState) };
        transitions[typeof(DodgeState)] = new HashSet<System.Type>() { typeof(HurtState), typeof(DeathState) };
        transitions[typeof(BlockState)] = new HashSet<System.Type>() { typeof(BlockHitState), typeof(HurtState), typeof(DeathState) };
        transitions[typeof(BlockHitState)] = new HashSet<System.Type>() { typeof(BlockState), typeof(HurtState), typeof(DeathState) };
        transitions[typeof(StunState)] = new HashSet<System.Type>() { typeof(HurtState), typeof(DeathState) };
        transitions[typeof(HurtState)] = new HashSet<System.Type>() { typeof(HurtState), typeof(DeathState) };
        transitions[typeof(DeathState)] = new HashSet<System.Type>() { typeof(ReviveState) };
        transitions[typeof(ReviveState)] = new HashSet<System.Type>();
        transitions[typeof(InteractState)] = new HashSet<System.Type>();
    }
}
