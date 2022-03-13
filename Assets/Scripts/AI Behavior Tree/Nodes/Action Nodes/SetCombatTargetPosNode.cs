using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Action node that sets the stored combat target's position as the navigation target.
    /// </summary>
    /// <remarks>
    /// <br><b>Success</b>: Always.</br>
    /// <br><b>Failure</b>: -</br>
    /// <br><b>Running</b>: -</br>
    /// </remarks>
    public class SetCombatTargetPosNode : BehaviorNode
    {
        private readonly Movement ownerMovement;

        public SetCombatTargetPosNode(Movement ownerMovement)
        {
            this.ownerMovement = ownerMovement;
        }

        public override NodeState Execute()
        {
            ActorController target = (ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);
            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, (Vector2)target.transform.position);
            Blackboard.SetData(
                GeneralBlackboardKeys.NAV_TARGET_STOPPING_DISTANCE,
                Mathf.Max(ownerMovement.DefaultStoppingDistanceFromNavTargets, target.Movement.DefaultStoppingDistanceFromNavTargets));

            return NodeState.SUCCESS;
        }
    }
}
