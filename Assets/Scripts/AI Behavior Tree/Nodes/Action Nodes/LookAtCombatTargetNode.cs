using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class LookAtCombatTargetNode : BehaviorNode
    {
        private readonly Detection ownerDetection;

        public LookAtCombatTargetNode(Detection ownerDetection)
        {
            this.ownerDetection = ownerDetection;
        }

        public override NodeState Execute()
        {
            object combatTargetObj = Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);
            if (combatTargetObj == null)
            {
                return NodeState.FAILURE;
            }

            ownerDetection.LookAt(((ActorController)combatTargetObj).transform.position);
            return NodeState.SUCCESS;
        }
    }
}
