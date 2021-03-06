using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirMovementStates
{
    public class AirborneState : AirMovementState
    {
        private bool isMoveRequestPending = false;
        private Vector2 moveDirection;

        public AirborneState(AirMovement owner) : base(owner) { }

        public override void Enter()
        {
            UpdateHorizontalMovement(moveDirection.x);
            UpdateVerticalMovement(moveDirection.y);
        }

        public override void Execute()
        {
            if (isMoveRequestPending)
            {
                isMoveRequestPending = false;
                owner.Rigidbody2d.velocity = moveDirection.normalized * owner.MovementSpeed;
                owner.Animator.SetBool("isFlying", moveDirection.x != 0f || moveDirection.y != 0f);
                owner.FlipDirection(moveDirection.x);
            }
        }

        public override void Exit()
        {
            owner.Animator.SetBool("isFlying", false);
        }

        public void UpdateHorizontalMovement(float direction)
        {
            isMoveRequestPending = true;
            moveDirection = new Vector2(direction, moveDirection.y);
        }

        public void UpdateVerticalMovement(float direction)
        {
            isMoveRequestPending = true;
            moveDirection = new Vector2(moveDirection.x, direction);
        }
    }
}
