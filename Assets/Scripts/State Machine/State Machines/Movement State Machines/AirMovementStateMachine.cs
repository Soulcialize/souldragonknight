using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AirMovementStates;

namespace StateMachines
{
    public class AirMovementStateMachine : StateMachine
    {
        public AirMovementStateMachine()
        {
            transitions[typeof(AirborneState)] = new HashSet<System.Type>();
        }
    }
}
