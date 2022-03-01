using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Action node that makes the actor ready an attack.
    /// </summary>
    /// <remarks>
    /// <br><b>Success</b>: Always.</br>
    /// <br><b>Failure</b>: -</br>
    /// <br><b>Running</b>: -</br>
    /// </remarks>
    public class ReadyAttackNode : BehaviorNode
    {
        private readonly Combat ownerCombat;

        public ReadyAttackNode(Combat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            ownerCombat.ReadyAttack();
            return NodeState.SUCCESS;
        }
    }
}
