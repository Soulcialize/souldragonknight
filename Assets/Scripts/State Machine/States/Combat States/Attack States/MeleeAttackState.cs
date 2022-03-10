using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class MeleeAttackState : AttackState
    {
        private readonly AttackEffectArea attackEffectArea;
        private readonly float readyAttackStartTime;

        public MeleeAttackState(Combat owner, AttackEffectArea attackEffectArea, float readyAttackStartTime) : base(owner)
        {
            this.attackEffectArea = attackEffectArea;
            this.readyAttackStartTime = readyAttackStartTime;
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
                    bool isActorHitFacingOwner =
                        actorHit.Movement.IsFacingRight && owner.transform.position.x > actorHit.transform.position.x
                        || !actorHit.Movement.IsFacingRight && owner.transform.position.x < actorHit.transform.position.x;

                    if (isActorHitFacingOwner && actorHit.Combat.CombatStateMachine.CurrState is BlockState blockState)
                    {
                        if (blockState.StartTime >= readyAttackStartTime)
                        {
                            owner.Stun();
                        }
                        else
                        {
                            blockState.Knockback(new Vector2(actorHit.Movement.IsFacingRight ? -1f : 1f, 0f));
                        }
                    }
                    else if (isActorHitFacingOwner && actorHit.Combat.CombatStateMachine.CurrState is ReadyAttackState)
                    {
                        Vector2 ownerKnockbackDirection = new Vector2(actorHit.Movement.IsFacingRight ? 1f : -1f, 0f);
                        owner.Clash(ownerKnockbackDirection);
                        actorHit.Combat.Clash(-ownerKnockbackDirection);
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
