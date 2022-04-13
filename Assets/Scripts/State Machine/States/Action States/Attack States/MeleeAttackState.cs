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
            AudioManagerSynced.Instance.PlaySoundFx(true, owner.SoundFXIndexLibrary.Attack);
        }

        public override void ExecuteAttackEffect()
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(
                attackEffectArea.transform.position,
                attackEffectArea.Size,
                attackEffectArea.transform.eulerAngles.z,
                owner.AttackEffectLayer);

            owner.Resource.Consume(attackCost);


            ActorController actorHit = null;
            BreakableWall breakableWall = null;
            foreach (Collider2D hit in hits)
            {
                // prioritize hit collider if it is in attacker's buffed target layer
                if (owner.Buff != null && GeneralUtility.IsLayerInLayerMask(hit.gameObject.layer, owner.Buff.BuffedTargetLayer))
                {
                    owner.RemoveBuff();
                    // assuming only actors are in the buffed target layer
                    ActorController.GetActorFromCollider(hit).Combat.HandleAttackHit(owner);
                    break;
                }

                if (actorHit == null)
                {
                    actorHit = ActorController.GetActorFromCollider(hit);
                    // move on to check for breakable wall if actor not found
                    if (actorHit != null)
                    {
                        Debug.Log("actor found");
                        continue;
                    }
                }

                if (breakableWall == null)
                {
                    breakableWall = hit.GetComponent<BreakableWall>();
                    continue;
                }
            }

            // only execute hit after looping through the whole list, since we want to prioritize buff targets
            if (actorHit != null)
            {
                owner.RemoveBuff();
                actorHit.Combat.HandleAttackHit(owner);
            }
            else if (breakableWall != null && breakableWall.CanAttackerBreak(owner))
            {
                owner.RemoveBuff();
                breakableWall.HandleHit();
            }
        }
    }
}
