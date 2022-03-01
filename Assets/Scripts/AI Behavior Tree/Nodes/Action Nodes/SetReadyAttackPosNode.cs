using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Action node that sets the position at which to ready an attack as the navigation target.
    /// </summary>
    /// <remarks>
    /// <br><b>Success</b>: Always.</br>
    /// <br><b>Failure</b>: -</br>
    /// <br><b>Running</b>: -</br>
    /// </remarks>
    public class SetReadyAttackPosNode : BehaviorNode
    {
        private readonly TouchCombat ownerCombat;

        public SetReadyAttackPosNode(TouchCombat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            Transform target = ((GameObject)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET)).transform;
            Vector2 directionToTarget = (target.position - ownerCombat.transform.position).normalized;
            Vector2 readyAttackPos = (Vector2)target.position + -directionToTarget * ownerCombat.ReadyAttackDistance;
            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, readyAttackPos);
            return NodeState.SUCCESS;
        }
    }
}
