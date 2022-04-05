using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;
using Pathfinding;

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
    public class GoToNavTargetNode : BehaviorNode
    {
        private readonly Transform ownerTransform;
        private readonly Movement ownerMovement;
        private readonly Combat ownerCombat;
        private readonly bool useStoppingDistance;

        /// <param name="ownerMovement">The actor's movement component.</param>
        /// <param name="ownerCombat">The actor's combat component</param>
        /// <param name="useStoppingDistance">
        /// If true, this node returns success when the actor is within
        /// its stopping distance (retrieved from the movement component) from the target position.
        /// Else, this node only returns success when the actor is almost exactly at the target position.
        /// </param>
        public GoToNavTargetNode(Movement ownerMovement, Combat ownerCombat, bool useStoppingDistance)
        {
            ownerTransform = ownerMovement.transform;
            this.ownerMovement = ownerMovement;
            this.ownerCombat = ownerCombat;
            this.useStoppingDistance = useStoppingDistance;
        }

        public override NodeState Execute()
        {
            Vector2 navTargetPos = (Vector2)Blackboard.GetData(GeneralBlackboardKeys.NAV_TARGET);
            Vector2 currentPos = ownerMovement.transform.position;

            float distanceToTarget = Vector2.Distance(currentPos, navTargetPos);
            if (useStoppingDistance && distanceToTarget <= (float)Blackboard.GetData(GeneralBlackboardKeys.NAV_TARGET_STOPPING_DISTANCE)
                || !useStoppingDistance && distanceToTarget <= 0.01f)
            {
                return NodeState.SUCCESS;
            }

            List<Node> pathToTarget = (List<Node>)Blackboard.GetData(GeneralBlackboardKeys.NAV_TARGET_PATH);
            NodeGrid.Instance.path = pathToTarget;
            if (pathToTarget == null)
            {
                return NodeState.FAILURE;
            }

            bool isDistanceGreaterThanWalkThreshold = distanceToTarget > ownerMovement.NavTargetWalkDistanceThreshold;
            if (ownerMovement.MovementMode == MovementSpeedData.Mode.SLOW && isDistanceGreaterThanWalkThreshold)
            {
                ownerMovement.SetMovementMode(MovementSpeedData.Mode.FAST);
            }
            else if (ownerMovement.MovementMode == MovementSpeedData.Mode.FAST && !isDistanceGreaterThanWalkThreshold)
            {
                ownerMovement.SetMovementMode(MovementSpeedData.Mode.SLOW);
            }

            Node currentPosNode = GetCurrentPositionAsNode();
            Node targetPathNextNode = pathToTarget[0];
            if (ownerMovement is GroundMovement groundMovement
                && targetPathNextNode.WorldPos.y > currentPosNode.WorldPos.y)
            {
                groundMovement.Jump();
            }
            else 
            {
                ownerMovement.UpdateMovement(targetPathNextNode.WorldPos - currentPosNode.WorldPos);
            }

            return NodeState.RUNNING;
        }

        private Node GetCurrentPositionAsNode()
        {
            return NodeGrid.Instance.GetNodeFromWorldPoint(
                new Vector2(ownerTransform.position.x, ownerCombat.Collider2d.bounds.max.y));
        }
    }
}
