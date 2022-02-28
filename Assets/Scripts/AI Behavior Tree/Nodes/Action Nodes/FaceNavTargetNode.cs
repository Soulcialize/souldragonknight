using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Action node that turns the actor to face its navigation target.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Meant to be used when the actor should face the target but not move towards it.
    /// The navigation target position is retrieved from the blackboard.
    /// </para>
    /// <br><b>Success</b>: Always.</br>
    /// <br><b>Failure</b>: -</br>
    /// <br><b>Running</b>: -</br>
    /// </remarks>
    public class FaceNavTargetNode : MovementNode
    {
        public FaceNavTargetNode(Movement ownerMovement) : base(ownerMovement) { }

        public override NodeState Execute()
        {
            Vector2 navTargetPos = (Vector3)Blackboard.GetData(GeneralBlackboardKeys.NAV_TARGET);
            Vector2 currentPos = ownerMovement.transform.position;

            if (navTargetPos.x > currentPos.x && !ownerMovement.IsFacingRight)
            {
                ownerMovement.FlipDirection(Movement.Direction.RIGHT);
            }
            else if (navTargetPos.x < currentPos.x && ownerMovement.IsFacingRight)
            {
                ownerMovement.FlipDirection(Movement.Direction.LEFT);
            }

            return NodeState.SUCCESS;
        }
    }
}
