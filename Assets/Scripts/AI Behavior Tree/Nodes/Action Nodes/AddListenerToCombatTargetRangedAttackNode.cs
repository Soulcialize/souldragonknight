using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class AddListenerToCombatTargetRangedAttackNode : BehaviorNode
    {
        private readonly Combat ownerCombat;

        public AddListenerToCombatTargetRangedAttackNode(Combat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            ActorController target = (ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);
            RangedAttackAbility targetAbility = (RangedAttackAbility)target.Combat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED);
            targetAbility.FireRangedProjectileEvent.AddListener(TargetFireRangedProjectileHandler);
            return NodeState.SUCCESS;
        }

        private void TargetFireRangedProjectileHandler(RangedProjectile projectile)
        {
            if (ownerCombat.GetCombatAbility(CombatAbilityIdentifier.BLOCK) == null
                || !((BlockAbility)ownerCombat.GetCombatAbility(CombatAbilityIdentifier.BLOCK)).CanBlockProjectiles)
            {
                // cannot block or cannot block projectiles
                return;
            }

            if (ShouldBlockProjectile(projectile))
            {
                // projectile is travelling towards actor, start block
                float projectileAngle = Vector2.Angle(Vector2.up, projectile.Direction);
                BlockState.Direction blockDirection = projectileAngle >= 135f
                    ? BlockState.Direction.UPWARDS
                    : BlockState.Direction.HORIZONTAL;

                projectile.HitEvent.AddListener(() => ownerCombat.EndCombatAbility(CombatAbilityIdentifier.BLOCK));
                ownerCombat.ExecuteCombatAbility(CombatAbilityIdentifier.BLOCK, blockDirection);
            }
        }

        private bool ShouldBlockProjectile(RangedProjectile projectile)
        {
            RaycastHit2D projectileRaycastHit = Physics2D.Raycast(
                projectile.transform.position, projectile.Direction,
                Mathf.Infinity, projectile.ActorTargetsLayer);

            // check if projectile is going to hit any actors at all
            if (projectileRaycastHit.collider == null)
            {
                return false;
            }

            // check if projectile is going to hit this actor
            ActorController hitActor = ActorController.GetActorFromCollider(projectileRaycastHit.collider);
            if (hitActor.Combat != ownerCombat)
            {
                return false;
            }

            return true;
        }
    }
}
