using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class IsCombatTargetInMeleeRangeNode : BehaviorNode
    {
        private readonly Transform ownerTransform;
        
        public IsCombatTargetInMeleeRangeNode(Movement ownerMovement)
        {
            ownerTransform = ownerMovement.transform;
        }

        public override NodeState Execute()
        {
            Vector2 targetPosition = ((ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET)).transform.position;
            return Vector2.Distance(ownerTransform.position, targetPosition) <= (float)Blackboard.GetData(GeneralBlackboardKeys.NAV_TARGET_STOPPING_DISTANCE)
                ? NodeState.SUCCESS
                : NodeState.FAILURE;
        }
    }
}
