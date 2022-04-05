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
        
        public IsCombatTargetInMeleeRangeNode(Movement ownerMovement)
        {
            ownerTransform = ownerMovement.transform;
            this.ownerMovement = ownerMovement;
        }

        public override NodeState Execute()
        {
            Bounds targetColliderBounds = ((ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET)).Combat.Collider2d.bounds;

            if (ownerMovement is GroundMovement groundMovement && targetColliderBounds.center.y > groundMovement.MaxReachableHeight)
            {
                return NodeState.FAILURE;
            }

            // only fail if target's entire collider is behind actor
            return ownerMovement.IsFacingRight && targetColliderBounds.max.x < ownerTransform.position.x
                || !ownerMovement.IsFacingRight && targetColliderBounds.min.x > ownerTransform.position.x
                ? NodeState.FAILURE
                : NodeState.SUCCESS;
        }
    }
}
