using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirMovementStates
{
    public class FallingState : AirMovementState
    {
        public FallingState(AirMovement owner) : base(owner) { }

        public override void Enter()
        {
            owner.Animator.SetBool("isFalling", true);
        }

        public override void Execute()
        {
            if (owner.GroundDetector.IsInContact)
            {
                owner.MovementStateMachine.ChangeState(new GroundedState(owner));
            }
        }

        public override void Exit()
        {
            owner.Animator.SetBool("isFalling", false);
        }
    }
}
