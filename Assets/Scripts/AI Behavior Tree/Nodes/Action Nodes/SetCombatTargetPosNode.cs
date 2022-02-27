using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class SetCombatTargetPosNode : BehaviorNode
    {
        public override NodeState Execute()
        {
            GameObject target = (GameObject)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);
            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, target.transform.position);
            return NodeState.SUCCESS;
        }
    }
}
