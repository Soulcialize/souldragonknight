using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class GetVisibleCombatTargetNode : BehaviorNode
    {
        public override NodeState Execute()
        {
            // TODO: use actual detection instead of grabbing player
            Blackboard.SetData(CombatBlackboardKeys.COMBAT_TARGET, Object.FindObjectOfType<KnightPlayerController>().gameObject);
            return NodeState.SUCCESS;
        }
    }
}
