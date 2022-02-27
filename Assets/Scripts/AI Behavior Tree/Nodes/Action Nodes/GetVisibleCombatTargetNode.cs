using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Action node that gets the nearest visible combat target and stores it in the blackboard.
    /// </summary>
    /// <remarks>
    /// <br><b>Success</b>: Visible target found and stored in the blackboard.</br>
    /// <br><b>Failure</b>: No visible target found.</br>
    /// <br><b>Running</b>: -</br>
    /// </remarks>
    public class GetVisibleCombatTargetNode : BehaviorNode
    {
        public override NodeState Execute()
        {
            // TODO: use actual detection instead of grabbing player
            Blackboard.SetData(CombatBlackboardKeys.COMBAT_TARGET, Object.FindObjectOfType<KnightPlayerController>().gameObject);
            return NodeState.SUCCESS;
        }
    }
}
