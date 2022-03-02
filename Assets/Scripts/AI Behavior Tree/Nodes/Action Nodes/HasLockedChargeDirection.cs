using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

namespace AiBehaviorTreeNodes
{
    public class HasLockedChargeDirection : BehaviorNode
    {
        private readonly TouchCombat ownerCombat;

        public HasLockedChargeDirection(TouchCombat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            return ((ReadyTouchAttackState)ownerCombat.CombatStateMachine.CurrState).HasLockedTargetPosition
                ? NodeState.SUCCESS
                : NodeState.FAILURE;
        }
    }
}
