using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GroundMovementStates
{
    public class GroundedState : GroundMovementState
    {
        private bool isMoveRequestPending = false;
        private float horizontalMoveDirection = 0f;
        private string currentMovementModeAnimatorParameter;

        private bool isJumpRequestPending = false;

        public GroundedState(GroundMovement owner) : base(owner) { }

        public override void Enter()
        {
            UpdateHorizontalMovement(owner.CachedMovementDirection.x);
            UpdateMovementMode(owner.MovementMode);
        }

        public override void Execute()
        {
            if (!owner.GroundDetector.IsInContact)
            {
                owner.MovementStateMachine.ChangeState(new AirborneState(owner));
            }
            else if (isJumpRequestPending)
            {
                AudioManagerSynced.Instance.PlaySoundFx(owner.SoundFXIndexLibrary.Jump);
                isJumpRequestPending = false;
                owner.Rigidbody2d.velocity = new Vector2(owner.Rigidbody2d.velocity.x, owner.JumpForce);
                owner.MovementStateMachine.ChangeState(new AirborneState(owner));
            }
            else if (isMoveRequestPending)
            {
                isMoveRequestPending = false;
                owner.Rigidbody2d.velocity = new Vector2(horizontalMoveDirection * owner.MovementSpeed, owner.Rigidbody2d.velocity.y);
                owner.Animator.SetBool(currentMovementModeAnimatorParameter, horizontalMoveDirection != 0f);
                owner.FlipDirection(horizontalMoveDirection);
            }
        }

        public override void Exit()
        {
            owner.Animator.SetBool("isRunning", false);
        }

        public void UpdateHorizontalMovement(float direction)
        {
            isMoveRequestPending = true;
            horizontalMoveDirection = Mathf.Clamp(direction, -1f, 1f);
        }

        public void UpdateMovementMode(MovementSpeedData.Mode mode)
        {
            string prevParameter = currentMovementModeAnimatorParameter;

            // update movement animator parameter
            switch (mode)
            {
                case MovementSpeedData.Mode.SLOW:
                    currentMovementModeAnimatorParameter = "isWalking";
                    break;
                case MovementSpeedData.Mode.FAST:
                    currentMovementModeAnimatorParameter = "isRunning";
                    break;
                default:
                    throw new System.ArgumentException(
                        $"{System.Enum.GetName(typeof(MovementSpeedData.Mode), mode)} not supported");
            }

            // update animator state
            if (horizontalMoveDirection != 0f)
            {
                owner.Animator.SetBool(currentMovementModeAnimatorParameter, true);
                if (prevParameter != null)
                {
                    owner.Animator.SetBool(prevParameter, false);
                }
            }
        }

        public void PostJumpRequest()
        {
            isJumpRequestPending = true;
        }
    }
}
