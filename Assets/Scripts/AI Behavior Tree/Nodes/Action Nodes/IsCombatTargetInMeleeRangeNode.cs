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
            ownerTransform = ownerCombat.transform;
            this.ownerMovement = ownerMovement;
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            Bounds targetColliderBounds = ((ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET)).Combat.Collider2d.bounds;

            // only fail if target's entire collider is behind actor
            return ownerMovement.IsFacingRight && targetColliderBounds.max.x < ownerTransform.position.x
                || !ownerMovement.IsFacingRight && targetColliderBounds.min.x > ownerTransform.position.x
                ? NodeState.FAILURE
                : NodeState.SUCCESS;
        }
    }
}
