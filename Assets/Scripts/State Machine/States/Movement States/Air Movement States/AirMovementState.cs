using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;

namespace AirMovementStates
{
    public abstract class AirMovementState : State
    {
        protected readonly AirMovement owner;

        public AirMovementState(AirMovement owner)
        {
            this.owner = owner;
        }
    }
}
