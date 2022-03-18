using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class MeleeAttackState : AttackState
    {
        private readonly AttackEffectArea attackEffectArea;

        public MeleeAttackState(Combat owner, AttackEffectArea attackEffectArea) : base(owner)
        {
            this.attackEffectArea = attackEffectArea;
        }

        public override void ExecuteAttackEffect()
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(
                attackEffectArea.transform.position,
                attackEffectArea.Size,
                attackEffectArea.transform.eulerAngles.z,
                owner.AttackEffectLayer);

            foreach (Collider2D hit in hits)
            {
                ActorController actorHit = ActorController.GetActorFromCollider(hit);
                if (actorHit != null)
                {
                    actorHit.Combat.HandleAttackHit(owner);
                    continue;
                }

                BreakableWall breakableWall = hit.GetComponent<BreakableWall>();
                if (breakableWall != null)
                {
                    owner.Debuff();
                    breakableWall.HandleHit();
                    continue;
                }
            }
        }
    }
}
