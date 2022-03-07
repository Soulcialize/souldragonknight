using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class IsCombatTargetMovingPastNode : BehaviorNode
    {
        private readonly Transform ownerTransform;
        private readonly MeleeCombat ownerCombat;

        public IsCombatTargetMovingPastNode(MeleeCombat ownerCombat)
        {
            ownerTransform = ownerCombat.transform;
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            ActorController combatTarget = ((GameObject)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET)).GetComponent<ActorController>();
            if (combatTarget == null)
            {
                return NodeState.FAILURE;
            }

            Transform targetTransform = combatTarget.transform;

            float distanceToTarget = Vector2.Distance(targetTransform.position, ownerTransform.position);
            bool isTargetMovingTowardsOwner =
                // moving rightwards towards owner
                combatTarget.Movement.IsFacingRight
                && combatTarget.Movement.Rigidbody2d.velocity.x > 0f
                && targetTransform.position.x < ownerTransform.position.x
                // or moving leftwards towards owner
                || !combatTarget.Movement.IsFacingRight
                && combatTarget.Movement.Rigidbody2d.velocity.x < 0f
                && targetTransform.position.x > ownerTransform.position.x;

            return distanceToTarget <= ownerCombat.DistanceBeforePreemptiveClash && isTargetMovingTowardsOwner
                ? NodeState.SUCCESS
                : NodeState.FAILURE;
        }
    }
}
