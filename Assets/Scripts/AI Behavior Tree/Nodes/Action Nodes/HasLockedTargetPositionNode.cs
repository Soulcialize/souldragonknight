using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

namespace AiBehaviorTreeNodes
{
    public class HasLockedTargetPositionNode : BehaviorNode
    {
        private readonly Combat ownerCombat;

        public HasLockedTargetPositionNode(Combat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            return ((ReadyRangedAttackState)ownerCombat.ActionStateMachine.CurrState).HasLockedTargetPosition
                ? NodeState.SUCCESS
                : NodeState.FAILURE;
        }
    }
}
