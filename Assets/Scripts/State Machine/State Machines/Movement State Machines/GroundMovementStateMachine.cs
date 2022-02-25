using System;
using System.Collections;
using System.Collections.Generic;
using GroundMovementStates;

namespace StateMachines
{
    public class GroundMovementStateMachine : StateMachine
    {
        public GroundMovementStateMachine()
        {
            transitions[typeof(GroundedState)] = new HashSet<Type>() { typeof(AirborneState) };
            transitions[typeof(AirborneState)] = new HashSet<Type>() { typeof(GroundedState) };
        }
    }
}
