using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class MeleeAttackState : AttackState
    {
        private new readonly MeleeCombat owner;
        private readonly float readyAttackStartTime;

        public MeleeAttackState(MeleeCombat owner, float readyAttackStartTime) : base(owner)
        {
            this.owner = owner;
            this.readyAttackStartTime = readyAttackStartTime;
        }

        public override void ExecuteAttackEffect()
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
                    bool isActorHitFacingOwner =
                        actorHit.Movement.IsFacingRight && owner.transform.position.x > actorHit.transform.position.x
                        || !actorHit.Movement.IsFacingRight && owner.transform.position.x < actorHit.transform.position.x;

                    if (isActorHitFacingOwner && actorHit.Combat.CombatStateMachine.CurrState is BlockState blockState)
                    {
                        if (blockState.StartTime >= readyAttackStartTime)
                        {
                            owner.Stun();
                        }
                    }
                    else
                    {
                        actorHit.Movement.UpdateMovement(Vector2.zero);
                        actorHit.Combat.Hurt();
                    }
                }
            }
        }
    }
}
