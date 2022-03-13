using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class IsCombatTargetInRangeNode : BehaviorNode
    {
        private readonly Transform ownerTransform;
        private readonly Movement ownerMovement;
        
        public IsCombatTargetInRangeNode(Movement ownerMovement)
        {
            ownerTransform = ownerMovement.transform;
            this.ownerMovement = ownerMovement;
        }

        public override NodeState Execute()
        {
            Vector2 targetPosition = ((GameObject)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET)).transform.position;
            return Vector2.Distance(ownerTransform.position, targetPosition) <= ownerMovement.GetStoppingDistanceFromNavTarget()
                ? NodeState.SUCCESS
                : NodeState.FAILURE;
        }
    }
}
