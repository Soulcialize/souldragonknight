using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Action node that moves the actor to the navigation target stored on the blackboard.
    /// </summary>
    /// <remarks>
    /// <br><b>Success</b>: When the actor reaches the navigation target position.</br>
    /// <br><b>Failure</b>: -</br>
    /// <br><b>Running</b>: While the actor is still navigating towards the target position.</br>
    /// </remarks>
    public class GoToNavTargetNode : MovementNode
    {
        private readonly bool useStoppingDistance;

        /// <param name="ownerMovement">The actor's movement component.</param>
        /// <param name="useStoppingDistance">
        /// If true, this node returns success when the actor is within
        /// its stopping distance (retrieved from the movement component) from the target position.
        /// Else, this node only returns success when the actor is almost exactly at the target position.
        /// </param>
        public GoToNavTargetNode(Movement ownerMovement, bool useStoppingDistance) : base(ownerMovement)
        {
            this.useStoppingDistance = useStoppingDistance;
        }

        public override NodeState Execute()
        {
            Vector2 navTargetPos = (Vector3)Blackboard.GetData(GeneralBlackboardKeys.NAV_TARGET);
            Vector2 currentPos = ownerMovement.transform.position;

            float distanceToTarget = Mathf.Abs(currentPos.x - navTargetPos.x);
            if (useStoppingDistance && distanceToTarget <= ownerMovement.GetStoppingDistanceFromNavTarget()
                || !useStoppingDistance && distanceToTarget <= 0.01f)
            {
                return NodeState.SUCCESS;
            }

            UpdateMovement(navTargetPos - currentPos);
            return NodeState.RUNNING;
        }
    }
}
