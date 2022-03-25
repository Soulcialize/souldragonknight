using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CombatStates
{
    public class BlockState : ActionState
    {
        public enum Direction { HORIZONTAL, UPWARDS }

        private readonly float blockHitDuration;
        private readonly Direction blockDirection;
        private readonly UnityEvent blockHitEvent;

        public Direction BlockDirection { get => blockDirection; }

        public BlockState(Combat owner, float blockHitDuration, Direction blockDirection, UnityEvent blockHitEvent) : base(owner)
        {
            this.blockHitDuration = blockHitDuration;
            this.blockDirection = blockDirection;
            this.blockHitEvent = blockHitEvent;
        }

        public override void Enter()
        {
            owner.Animator.SetBool("isBlocking", true);
            if (blockDirection == Direction.UPWARDS)
            {
                owner.Animator.SetBool("isBlockingUpwards", true);
            }
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            owner.Animator.SetBool("isBlocking", false);
            if (blockDirection == Direction.UPWARDS)
            {
                owner.Animator.SetBool("isBlockingUpwards", false);
            }
        }

        public void HandleHit(bool isOwnerFacingRight, Vector2 hitDirection)
        {
            float hitAngle = Vector2.Angle(Vector2.up, hitDirection);
            Direction hitBlockDirection = hitAngle > 45f && hitAngle < 135f ? Direction.HORIZONTAL : Direction.UPWARDS;

            if (blockDirection != hitBlockDirection)
            {
                owner.Hurt();
                return;
            }

            if (blockDirection == Direction.HORIZONTAL)
            {
                // check if owner is facing the right the direction from which the hit is coming from
                if (isOwnerFacingRight && hitDirection.x < 0f || !isOwnerFacingRight && hitDirection.x > 0f)
                {
                    owner.ActionStateMachine.ChangeState(new BlockHitState(owner, blockHitDuration, blockDirection, blockHitEvent));
                }
                else
                {
                    owner.Hurt();
                }
            }
            else if (blockDirection == Direction.UPWARDS)
            {
                owner.ActionStateMachine.ChangeState(new BlockHitState(owner, blockHitDuration, blockDirection, blockHitEvent));
            }
        }
    }
}
