using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class BlockKnockbackState : CombatKnockbackState
    {
        private new readonly MeleeCombat owner;
        private readonly float originalBlockStartTime;

        public BlockKnockbackState(MeleeCombat owner, Vector2 direction, float originalBlockStartTime) : base(owner, direction)
        {
            this.owner = owner;
            this.originalBlockStartTime = originalBlockStartTime;
        }

        public override void Enter()
        {
            base.Enter();
            owner.Animator.SetBool("isBlockKnockback", true);
        }

        public override void Exit()
        {
            base.Exit();
            owner.Animator.SetBool("isBlockKnockback", false);
        }

        protected override void EndKnockback()
        {
            base.EndKnockback();
            if (!owner.WallCollisionDetector.IsInContact)
            {
                owner.CombatStateMachine.ChangeState(new BlockState(owner, originalBlockStartTime));
            }
        }
    }
}
