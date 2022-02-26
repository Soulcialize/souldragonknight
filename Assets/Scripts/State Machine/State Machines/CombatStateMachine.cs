using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using CombatStates;

public class CombatStateMachine : StateMachine
{
    public CombatStateMachine()
    {
        transitions[typeof(AttackState)] = new HashSet<System.Type>() { typeof(HurtState) };
        transitions[typeof(HurtState)] = new HashSet<System.Type>() { typeof(HurtState) };
    }
}
