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
            ActorController target = (ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);
            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, (Vector2)target.transform.position);
            return NodeState.SUCCESS;
        }
    }
}
