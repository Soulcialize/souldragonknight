using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class MeleeAttackState : AttackState
    {
        private new readonly MeleeCombat owner;

        public MeleeAttackState(MeleeCombat owner) : base(owner)
        {
            this.owner = owner;
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
                    actorHit.Movement.UpdateMovement(Vector2.zero);
                    actorHit.Combat.Hurt();
                }
            }
        }
    }
}
