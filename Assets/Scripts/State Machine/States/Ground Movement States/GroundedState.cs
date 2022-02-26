using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GroundMovementStates
{
    public class GroundedState : GroundMovementState
    {
        public GroundedState(GroundMovement owner) : base(owner) { }

        private float horizontalMoveDirection = 0f;

        private bool isJumpRequestPending = false;

        public override void Enter()
        {
            UpdateHorizontalMovement(owner.CachedHorizontalMovementDirection);
        }

        public override void Execute()
        {
            if (!owner.GroundDetector.IsInContact)
            {
                owner.MovementStateMachine.ChangeState(new AirborneState(owner));
            }
            else if (isJumpRequestPending)
            {
                isJumpRequestPending = false;
                owner.Rigidbody2d.velocity = new Vector2(owner.Rigidbody2d.velocity.x, owner.JumpForce);
                owner.MovementStateMachine.ChangeState(new AirborneState(owner));
            }
            else
            {
                owner.Rigidbody2d.velocity = new Vector2(horizontalMoveDirection * owner.HorizontalMoveSpeed, owner.Rigidbody2d.velocity.y);
                owner.Animator.SetBool("isRunning", horizontalMoveDirection != 0f);
                owner.FlipDirection(horizontalMoveDirection);
            }
        }

        public override void Exit()
        {
            owner.Animator.SetBool("isRunning", false);
        }

        public void UpdateHorizontalMovement(float direction)
        {
            horizontalMoveDirection = direction;
        }

        public void PostJumpRequest()
        {
            isJumpRequestPending = true;
        }
    }
}
