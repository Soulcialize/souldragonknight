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
    public class StopMovingNode : BehaviorNode
    {
        private readonly ActorController owner;

        public StopMovingNode(ActorController owner)
        {
            this.owner = owner;
        }

        public override NodeState Execute()
        {
            owner.Pathfinder.StopPathfind();
            owner.Movement.UpdateMovement(Vector2.zero);
            return NodeState.SUCCESS;
        }
    }
}
