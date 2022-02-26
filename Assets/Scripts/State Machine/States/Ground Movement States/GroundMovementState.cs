using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;

namespace GroundMovementStates
{
    public abstract class GroundMovementState : State
    {
        protected readonly GroundMovement owner;

        public GroundMovementState(GroundMovement owner)
        {
            this.owner = owner;
        }
    }
}
