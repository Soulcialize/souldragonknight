using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class CanReachNavTargetNode : BehaviorNode
    {
        private readonly Movement ownerMovement;

        public CanReachNavTargetNode(Movement ownerMovement)
        {
            this.ownerMovement = ownerMovement;
        }

        public override NodeState Execute()
        {
            Vector2 navTarget = (Vector2)Blackboard.GetData(GeneralBlackboardKeys.NAV_TARGET);

            if (ownerMovement is AirMovement)
            {
                return NodeState.SUCCESS;
            }
            
            if (ownerMovement is GroundMovement groundMovement)
            {
                Debug.Log($"{ownerMovement.gameObject}: {navTarget}");
                RaycastHit2D groundHit = Physics2D.Raycast(
                    navTarget, Vector2.down, Mathf.Infinity, ownerMovement.GroundDetector.SurfacesLayerMask);

                return groundHit.distance > groundMovement.MaxReachableHeight
                    ? NodeState.FAILURE
                    : NodeState.SUCCESS;
            }

            throw new System.ArgumentException($"Movement type {ownerMovement.GetType()} not accounted for");
        }
    }
}
