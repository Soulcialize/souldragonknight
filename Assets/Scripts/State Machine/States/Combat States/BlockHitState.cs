using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class BlockHitState : CombatState
    {
        private readonly float duration;

        private float startTime;
        private bool willReturnToBlock;

        public bool WillReturnToBlock
        {
            get => willReturnToBlock;
            set
            {
                willReturnToBlock = value;
                owner.Animator.SetBool("isBlocking", value);
            }
        }

        public BlockHitState(Combat owner, float duration) : base(owner)
        {
            this.duration = duration;
        }

        public override void Enter()
        {
            startTime = Time.time;
            WillReturnToBlock = true;

            owner.Animator.SetBool("isBlockingHit", true);
        }

        public override void Execute()
        {
            if (Time.time - startTime > duration)
            {
                // switch to block state or exit combat state machine
                if (WillReturnToBlock)
                {
                    owner.CombatStateMachine.ChangeState(new BlockState(owner, duration));
                }
                else
                {
                    owner.CombatStateMachine.Exit();
                }
            }
        }

        public override void Exit()
        {
            owner.Animator.SetBool("isBlockingHit", false);
        }
    }
}
