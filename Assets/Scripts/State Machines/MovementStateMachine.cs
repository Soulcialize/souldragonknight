using System;
using System.Collections;
using System.Collections.Generic;

public class MovementStateMachine : StateMachine
{
    public MovementStateMachine()
    {
        transitions[typeof(GroundedState)]  = new HashSet<Type>() { typeof(AirborneState), typeof(KnockbackState) };
        transitions[typeof(AirborneState)]  = new HashSet<Type>() { typeof(GroundedState), typeof(KnockbackState) };
        transitions[typeof(KnockbackState)] = new HashSet<Type>() { typeof(GroundedState), typeof(AirborneState) };
    }
}
