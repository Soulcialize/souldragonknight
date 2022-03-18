using System;
using System.Collections;
using System.Collections.Generic;
using GroundMovementStates;

namespace StateMachines
{
    public class GroundMovementStateMachine : MovementStateMachine
    {
        public GroundMovementStateMachine()
        {
            transitions[typeof(GroundedState)] = new HashSet<Type>() { typeof(AirborneState), typeof(MountedState) };
            transitions[typeof(AirborneState)] = new HashSet<Type>() { typeof(GroundedState), typeof(MountedState) };
            transitions[typeof(MountedState)] = new HashSet<Type>() { typeof(AirborneState) };
        }
    }
}
