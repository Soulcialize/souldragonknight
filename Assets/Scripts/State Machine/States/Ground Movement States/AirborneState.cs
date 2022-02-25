using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GroundMovementStates
{
    public class AirborneState : GroundMovementState
    {
        public AirborneState(GroundMovement owner) : base(owner) { }

        private bool isFalling = false;

        public override void Enter()
        {
            isFalling = ShouldStartFalling();
            owner.Animator.SetBool("isJumping", !isFalling);
            owner.Animator.SetBool("isFalling", isFalling);
        }

        public override void Execute()
        {
            if (isFalling)
            {
                if (owner.GroundDetector.IsInContact)
                {
                    owner.MovementStateMachine.ChangeState(new GroundedState(owner));
                }
            }
            else if (ShouldStartFalling())
            {
                isFalling = true;
                owner.Animator.SetBool("isJumping", false);
                owner.Animator.SetBool("isFalling", true);
            }
        }

        public override void Exit()
        {
            owner.Animator.SetBool("isJumping", false);
            owner.Animator.SetBool("isFalling", false);
        }

        private bool ShouldStartFalling()
        {
            return owner.Rigidbody2d.velocity.y <= owner.JumpForce / 2f;
        }
    }
}
