using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirMovementStates
{
    public class AirborneState : AirMovementState
    {
        private float horizontalMoveDirection = 0f;
        private float verticalMoveDirection = 0f;

        public AirborneState(AirMovement owner) : base(owner) { }

        public override void Enter()
        {
            UpdateHorizontalMovement(owner.CachedHorizontalMoveDirection);
            UpdateVerticalMovement(owner.CachedVerticalMoveDirection);
        }

        public override void Execute()
        {
            owner.Rigidbody2d.velocity = new Vector2(horizontalMoveDirection, verticalMoveDirection).normalized * owner.MovementSpeed;
            owner.Animator.SetBool("isFlying", horizontalMoveDirection != 0f || verticalMoveDirection != 0f);
            owner.FlipDirection(horizontalMoveDirection);
        }

        public override void Exit()
        {
            owner.Animator.SetBool("isFlying", false);
        }

        public void UpdateHorizontalMovement(float direction)
        {
            horizontalMoveDirection = direction;
        }

        public void UpdateVerticalMovement(float direction)
        {
            verticalMoveDirection = direction;
        }
    }
}
