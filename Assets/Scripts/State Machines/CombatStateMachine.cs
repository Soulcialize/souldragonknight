using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateMachine : StateMachine
{
    public CombatStateMachine()
    {
        transitions[typeof(AttackState)] = new HashSet<Type>() { typeof(StunState), typeof(HurtState), typeof(DeadState) };
        transitions[typeof(StunState)]   = new HashSet<Type>() { typeof(HurtState), typeof(DeadState) };
        transitions[typeof(HurtState)]   = new HashSet<Type>() { typeof(HurtState), typeof(DeadState) };
        transitions[typeof(DeadState)] = new HashSet<Type>();
    }
}
