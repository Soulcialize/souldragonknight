using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GroundMovementStates
{
    public class AirborneState : GroundMovementState
    {
        private readonly float maxHorizontalSpeed;

        private bool isFalling = false;

        private bool isMoveRequestPending = false;
        private float horizontalMoveDirection = 0f;

        public AirborneState(GroundMovement owner) : base(owner)
        {
            maxHorizontalSpeed = owner.MovementSpeed;
        }

        public override void Enter()
        {
            isFalling = ShouldStartFalling();
            owner.Animator.SetBool("isJumping", !isFalling);
            owner.Animator.SetBool("isFalling", isFalling);
        }

        public override void Execute()
        {
            if (isFalling && owner.GroundDetector.IsInContact)
            {
                owner.MovementStateMachine.ChangeState(new GroundedState(owner));
                return;
            }

            if (!isFalling && ShouldStartFalling())
            {
                isFalling = true;
                owner.Animator.SetBool("isJumping", false);
                owner.Animator.SetBool("isFalling", true);
            }

            if (isMoveRequestPending)
            {
                isMoveRequestPending = false;
                owner.Rigidbody2d.velocity = new Vector2(
                    Mathf.Clamp(
                        owner.Rigidbody2d.velocity.x + horizontalMoveDirection * owner.AirborneHorizontalMoveSpeed,
                        -maxHorizontalSpeed,
                        maxHorizontalSpeed),
                    owner.Rigidbody2d.velocity.y);
            }
        }

        public override void Exit()
        {
            owner.Animator.SetBool("isJumping", false);
            owner.Animator.SetBool("isFalling", false);
        }

        public void UpdateHorizontalMovement(float direction)
        {
            if (owner.IsFacingRight && direction > 0f || !owner.IsFacingRight && direction < 0f)
            {
                isMoveRequestPending = true;
                horizontalMoveDirection = Mathf.Clamp(direction, -1f, 1f);
            }
        }

        private bool ShouldStartFalling()
        {
            return owner.Rigidbody2d.velocity.y <= owner.JumpForce / 2f;
        }
    }
}
