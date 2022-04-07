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
        private readonly ActorController owner;

        /// <param name="ownerMovement">The actor's movement component.</param>
        /// <param name="ownerCombat">The actor's combat component</param>
        /// <param name="useStoppingDistance">
        /// If true, this node returns success when the actor is within
        /// its stopping distance (retrieved from the movement component) from the target position.
        /// Else, this node only returns success when the actor is almost exactly at the target position.
        /// </param>
        public GoToNavTargetNode(ActorController owner)
        {
            this.owner = owner;
        }

        public override NodeState Execute()
        {
            Vector2 navTargetPos = (Vector2)Blackboard.GetData(GeneralBlackboardKeys.NAV_TARGET);

            owner.Pathfinder.Pathfind(navTargetPos);
            if (owner.Pathfinder.LastPathfindResult == Pathfinder.PathfindResult.FAILURE)
            {
                // no path to reach or move nearer to target at all
                return NodeState.FAILURE;
            }
            else if (owner.Pathfinder.HasReachedFinalPathNode)
            {
                // reached final node in path
                return NodeState.SUCCESS;
            }

            // there is a path to the target or a path to a position that is nearer to the target than the current position
            float distanceToTarget = Vector2.Distance(owner.Pathfinder.GetCurrentPosForPathfinding(), navTargetPos);
            if (owner.Movement.MovementMode == MovementSpeedData.Mode.SLOW
                && distanceToTarget > owner.Movement.NavTargetFastDistanceThreshold)
            {
                owner.Movement.SetMovementMode(MovementSpeedData.Mode.FAST);
            }
            else if (owner.Movement.MovementMode == MovementSpeedData.Mode.FAST
                && distanceToTarget <= owner.Movement.NavTargetSlowDistanceThreshold)
            {
                owner.Movement.SetMovementMode(MovementSpeedData.Mode.SLOW);
            }

            return NodeState.RUNNING;
        }
    }
}
