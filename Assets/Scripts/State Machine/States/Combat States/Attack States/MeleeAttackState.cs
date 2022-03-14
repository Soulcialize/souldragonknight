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
                    actorHit.Movement.UpdateMovement(Vector2.zero);
                    if (actorHit.Combat.CombatStateMachine.CurrState is BlockState blockState)
                    {
                        blockState.HandleHit(actorHit.Movement.IsFacingRight, (actorHit.transform.position - owner.transform.position).normalized);
                    }
                    else
                    {
                        actorHit.Combat.Hurt();
                    }
                }
            }
        }
    }
}
