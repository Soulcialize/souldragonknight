using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Action node that stops the actor at its current position.
    /// </summary>
    /// <remarks>
    /// <br><b>Success</b>: Always.</br>
    /// <br><b>Failure</b>: -</br>
    /// <br><b>Running</b>: -</br>
    /// </remarks>
    public class StopMovingNode : MovementNode
    {
        public StopMovingNode(Movement ownerMovement) : base(ownerMovement) { }

        public override NodeState Execute()
        {
            UpdateMovement(Vector2.zero);
            return NodeState.SUCCESS;
        }
    }
}
