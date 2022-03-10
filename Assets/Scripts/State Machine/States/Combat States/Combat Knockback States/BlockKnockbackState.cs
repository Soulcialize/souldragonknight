using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class BlockKnockbackState : CombatKnockbackState
    {
        public BlockKnockbackState(Combat owner, Vector2 direction) : base(owner, direction) { }

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
                owner.CombatStateMachine.ChangeState(new BlockState(owner));
            }
        }
    }
}
