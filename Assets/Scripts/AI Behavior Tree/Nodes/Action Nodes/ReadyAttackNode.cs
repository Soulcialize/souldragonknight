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
        protected readonly Combat ownerCombat;

        public ReadyAttackNode(Combat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            Blackboard.SetData(CombatBlackboardKeys.HAS_READIED_ATTACK, false);
            ownerCombat.ReadyAttackEndEvent.RemoveListener(SetAttackReadied);
            ownerCombat.ReadyAttackEndEvent.AddListener(SetAttackReadied);

            ownerCombat.ReadyAttack(((GameObject)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET)).transform);
            return NodeState.SUCCESS;
        }

        private void SetAttackReadied()
        {
            Blackboard.SetData(CombatBlackboardKeys.HAS_READIED_ATTACK, true);
        }
    }
}
