using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class BlockKnockbackState : CombatState
    {
        private new readonly MeleeCombat owner;
        private readonly Vector2 direction;
        private readonly float originalBlockStartTime;

        private Vector2 startPos;

        public BlockKnockbackState(MeleeCombat owner, Vector2 direction, float originalBlockStartTime) : base(owner)
        {
            this.owner = owner;
            this.direction = direction;
            this.originalBlockStartTime = originalBlockStartTime;
        }

        public override void Enter()
        {
            startPos = owner.transform.position;
            owner.Animator.SetBool("isBlockKnockback", true);
        }

        public override void Execute()
        {
            owner.Rigidbody2d.velocity = direction * owner.BlockKnockbackSpeed;
            if (Vector2.Distance(startPos, owner.transform.position) > owner.BlockKnockbackDistance)
            {
                owner.Rigidbody2d.velocity = Vector2.zero;
                owner.CombatStateMachine.ChangeState(new BlockState(owner, originalBlockStartTime));
            }
        }

        public override void Exit()
        {
            owner.Animator.SetBool("isBlockKnockback", false);
        }
    }
}
