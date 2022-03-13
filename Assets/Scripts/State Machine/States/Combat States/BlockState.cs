using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class BlockState : CombatState
    {
        private readonly float blockHitDuration;

        public BlockState(Combat owner, float blockHitDuration) : base(owner)
        {
            this.blockHitDuration = blockHitDuration;
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

        public void HandleHit()
        {
            owner.CombatStateMachine.ChangeState(new BlockHitState(owner, blockHitDuration));
        }
    }
}
