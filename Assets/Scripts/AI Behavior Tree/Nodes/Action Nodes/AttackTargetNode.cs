using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Action node that makes the actor attack after a delay.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The delay (minimum time between attacks) is retrieved from the actor's combat component.
    /// The current time is retrieved from the blackboard.
    /// </para>
    /// <br><b>Success</b>: When the minimum time has passed and the attack starts.</br>
    /// <br><b>Failure</b>: -</br>
    /// <br><b>Running</b>: While the minimum time has not passed.</br>
    /// </remarks>
    public class AttackTargetNode : BehaviorNode
    {
        private readonly Combat ownerCombat;

        public AttackTargetNode(Combat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            if ((float)Blackboard.GetData(CombatBlackboardKeys.TIME_SINCE_LAST_ATTACK) < ownerCombat.MinTimeBetweenAttacks)
            {
                return NodeState.RUNNING;
            }

            Blackboard.SetData(CombatBlackboardKeys.TIME_SINCE_LAST_ATTACK, 0f);
            ownerCombat.Attack();
            return NodeState.SUCCESS;
        }
    }
}
