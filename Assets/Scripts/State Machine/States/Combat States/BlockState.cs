using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class BlockState : CombatState
    {
        public enum Direction { HORIZONTAL, UPWARDS }

        private readonly float blockHitDuration;
        private readonly Direction blockDirection;

        public Direction BlockDirection { get => blockDirection; }

        public BlockState(Combat owner, float blockHitDuration, Direction blockDirection) : base(owner)
        {
            this.blockHitDuration = blockHitDuration;
            this.blockDirection = blockDirection;
        }

        public override void Enter()
        {
            owner.Animator.SetBool("isBlocking", true);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            owner.Animator.SetBool("isBlocking", false);
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
                    owner.CombatStateMachine.ChangeState(new BlockHitState(owner, blockHitDuration, blockDirection));
                }
                else
                {
                    owner.Hurt();
                }
            }
            else if (blockDirection == Direction.UPWARDS)
            {
                owner.CombatStateMachine.ChangeState(new BlockHitState(owner, blockHitDuration, blockDirection));
            }
        }
    }
}
