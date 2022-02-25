using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class AttackState : CombatState
    {
        private readonly bool isFacingRight;

        public AttackState(Combat owner, bool isFacingRight) : base(owner)
        {
            this.isFacingRight = isFacingRight;
        }

        public override void Enter()
        {
            owner.Animator.SetBool("isAttacking", true);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            owner.Animator.SetBool("isAttacking", false);
        }

        public void ExecuteAttackEffect()
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(
                (isFacingRight ? owner.AttackEffectArea.RightOrigin : owner.AttackEffectArea.LeftOrigin).position,
                owner.AttackEffectArea.Size,
                owner.AttackEffectArea.LeftOrigin.eulerAngles.z,
                owner.AttackEffectLayer);

            foreach (Collider2D hit in hits)
            {
                ActorController actorHit = hit.GetComponent<ActorController>();
                if (actorHit != null)
                {
                    actorHit.Combat.Hurt();
                }
            }
        }
    }
}
