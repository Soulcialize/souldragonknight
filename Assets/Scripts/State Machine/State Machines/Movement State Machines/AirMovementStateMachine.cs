using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AirMovementStates;

namespace StateMachines
{
    public class AirMovementStateMachine : MovementStateMachine
    {
        public AirMovementStateMachine()
        {
            transitions[typeof(AirborneState)] = new HashSet<System.Type>() { typeof(FallingState), typeof(GroundedState) };
            transitions[typeof(FallingState)] = new HashSet<System.Type>() { typeof(GroundedState) };
            transitions[typeof(GroundedState)] = new HashSet<System.Type>();
        }
    }
}
