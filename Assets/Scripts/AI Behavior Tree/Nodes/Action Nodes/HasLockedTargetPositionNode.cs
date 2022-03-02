using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

namespace AiBehaviorTreeNodes
{
    public class HasLockedTargetPositionNode : BehaviorNode
    {
        private readonly ChargeCombat ownerCombat;

        public HasLockedTargetPositionNode(ChargeCombat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            return ((ReadyChargeAttackState)ownerCombat.CombatStateMachine.CurrState).HasLockedTargetPosition
                ? NodeState.SUCCESS
                : NodeState.FAILURE;
        }
    }
}
