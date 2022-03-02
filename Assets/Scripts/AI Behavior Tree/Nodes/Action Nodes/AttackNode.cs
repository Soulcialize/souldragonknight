using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class AttackNode : BehaviorNode
    {
        protected readonly Combat ownerCombat;

        public AttackNode(Combat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            if ((bool)Blackboard.GetData(CombatBlackboardKeys.HAS_READIED_ATTACK))
            {
                ownerCombat.Attack();
                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }
    }
}
