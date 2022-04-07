using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class IsCombatTargetInMeleeRangeNode : BehaviorNode
    {
        private readonly Transform ownerTransform;
        private readonly Movement ownerMovement;
        private readonly Combat ownerCombat;

        public IsCombatTargetInMeleeRangeNode(Movement ownerMovement, Combat ownerCombat)
        {
            ownerTransform = ownerMovement.transform;
            this.ownerMovement = ownerMovement;
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            Collider2D targetCollider = ((ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET)).Combat.Collider2d;

            if (ownerMovement is GroundMovement groundMovement && targetCollider.bounds.center.y > groundMovement.MaxReachableHeight)
            {
                // target is too high above
                return NodeState.FAILURE;
            }

            AttackEffectArea attackEffectArea =
                ((MeleeAttackAbility)ownerCombat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_MELEE)).AttackEffectArea;

            // check if target's collider is within width of attack effect area on either side
            return ownerTransform.position.x + attackEffectArea.TopCornerPos.x < targetCollider.bounds.min.x
                || ownerTransform.position.x - attackEffectArea.TopCornerPos.x > targetCollider.bounds.max.x
                ? NodeState.FAILURE
                : NodeState.SUCCESS;
        }
    }
}
