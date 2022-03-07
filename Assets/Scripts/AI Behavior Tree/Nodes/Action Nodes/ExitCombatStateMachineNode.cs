using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiBehaviorTreeNodes
{
    public class ExitCombatStateMachineNode : BehaviorNode
    {
        private readonly Combat ownerCombat;

        public ExitCombatStateMachineNode(Combat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            ownerCombat.CombatStateMachine.Exit();
            return NodeState.SUCCESS;
        }
    }
}
