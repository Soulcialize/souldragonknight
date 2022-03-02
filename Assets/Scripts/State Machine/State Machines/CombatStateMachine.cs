using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using CombatStates;

public class CombatStateMachine : StateMachine
{
    public CombatStateMachine()
    {
        transitions[typeof(ReadyAttackState)] = new HashSet<System.Type>() { typeof(AttackState), typeof(HurtState) };
        transitions[typeof(AttackState)] = new HashSet<System.Type>();
        transitions[typeof(HurtState)] = new HashSet<System.Type>() { typeof(HurtState) };
    }
}
