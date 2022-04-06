using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;
using Pathfinding;

using Filter = System.Predicate<Pathfinding.Node>;

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
        private readonly bool useStoppingDistance;

        /// <param name="ownerMovement">The actor's movement component.</param>
        /// <param name="ownerCombat">The actor's combat component</param>
        /// <param name="useStoppingDistance">
        /// If true, this node returns success when the actor is within
        /// its stopping distance (retrieved from the movement component) from the target position.
        /// Else, this node only returns success when the actor is almost exactly at the target position.
        /// </param>
        public GoToNavTargetNode(ActorController owner, bool useStoppingDistance)
        {
            this.owner = owner;
            this.useStoppingDistance = useStoppingDistance;
        }

        public override NodeState Execute()
        {
            Vector2 navTargetPos = (Vector2)Blackboard.GetData(GeneralBlackboardKeys.NAV_TARGET);
            Vector2 currentPos = owner.Pathfinder.GetCurrentPos();

            float distanceToTarget = Vector2.Distance(currentPos, navTargetPos);
            if (useStoppingDistance && distanceToTarget <= (float)Blackboard.GetData(GeneralBlackboardKeys.NAV_TARGET_STOPPING_DISTANCE)
                || !useStoppingDistance && distanceToTarget <= 0.01f)
            {
                owner.Pathfinder.StopPathfind();
                return NodeState.SUCCESS;
            }

            Pathfinder.PathfindResult pathfindResult = owner.Pathfinder.Pathfind(navTargetPos);
            if (pathfindResult == Pathfinder.PathfindResult.FAILURE)
            {
                // no path to reach or move nearer to target at all
                owner.Pathfinder.StopPathfind();
                return NodeState.FAILURE;
            }

            // there is a path to the target
            bool isDistanceGreaterThanWalkThreshold = distanceToTarget > owner.Movement.NavTargetWalkDistanceThreshold;
            if (owner.Movement.MovementMode == MovementSpeedData.Mode.SLOW && isDistanceGreaterThanWalkThreshold)
            {
                owner.Movement.SetMovementMode(MovementSpeedData.Mode.FAST);
            }
            else if (owner.Movement.MovementMode == MovementSpeedData.Mode.FAST && !isDistanceGreaterThanWalkThreshold)
            {
                owner.Movement.SetMovementMode(MovementSpeedData.Mode.SLOW);
            }

            return NodeState.RUNNING;
        }
    }
}
