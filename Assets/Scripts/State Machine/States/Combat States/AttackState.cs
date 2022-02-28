using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class AttackState : CombatState
    {
        public AttackState(Combat owner) : base(owner) { }

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
                owner.AttackEffectArea.transform.position,
                owner.AttackEffectArea.Size,
                owner.AttackEffectArea.transform.eulerAngles.z,
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
