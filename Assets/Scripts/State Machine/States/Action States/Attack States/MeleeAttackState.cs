using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class MeleeAttackState : AttackState
    {
        private readonly AttackEffectArea attackEffectArea;
        private readonly float attackCost;

        public MeleeAttackState(Combat owner, AttackEffectArea attackEffectArea, float attackCost) : base(owner)
        {
            this.attackEffectArea = attackEffectArea;
            this.attackCost = attackCost;
        }

        public override void Enter()
        {
            base.Enter();
            AudioManagerSynced.Instance.PlaySoundFx(owner.SoundFXIndexLibrary.Attack);
        }

        public override void ExecuteAttackEffect()
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(
                attackEffectArea.transform.position,
                attackEffectArea.Size,
                attackEffectArea.transform.eulerAngles.z,
                owner.AttackEffectLayer);

            owner.Resource.Consume(attackCost);

            foreach (Collider2D hit in hits)
            {
                ActorController actorHit = ActorController.GetActorFromCollider(hit);
                if (actorHit != null)
                {
                    owner.Debuff();
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
