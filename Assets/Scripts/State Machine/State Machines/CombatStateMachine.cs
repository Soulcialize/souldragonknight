using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using CombatStates;

public class CombatStateMachine : StateMachine
{
    public CombatStateMachine()
    {
        transitions[typeof(ReadyAttackState)] = new HashSet<System.Type>() { typeof(MeleeAttackState), typeof(TouchAttackState), typeof(HurtState) };
        transitions[typeof(MeleeAttackState)] = new HashSet<System.Type>();
        transitions[typeof(TouchAttackState)] = new HashSet<System.Type>();
        transitions[typeof(HurtState)] = new HashSet<System.Type>() { typeof(HurtState) };
    }
}
