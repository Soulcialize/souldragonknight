using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class CanReachNavTargetNode : BehaviorNode
    {
        private readonly Movement ownerMovement;
        private readonly Transform ownerTransform;

        public CanReachNavTargetNode(Movement ownerMovement)
        {
            this.ownerMovement = ownerMovement;
            ownerTransform = ownerMovement.transform;
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
                RaycastHit2D navTargetGroundHit = Physics2D.Raycast(
                    navTarget, Vector2.down, Mathf.Infinity, ownerMovement.GroundDetector.SurfacesLayerMask);

                RaycastHit2D ownerGroundHit = Physics2D.Raycast(
                    ownerTransform.position, Vector2.down, Mathf.Infinity, ownerMovement.GroundDetector.SurfacesLayerMask);

                bool isNavTargetOnHigherGround = navTargetGroundHit.point.y > ownerGroundHit.point.y;
                bool isNavTargetTooHighUp = navTargetGroundHit.distance - ownerGroundHit.distance > groundMovement.MaxReachableHeight;
                return isNavTargetOnHigherGround || isNavTargetTooHighUp
                    ? NodeState.FAILURE
                    : NodeState.SUCCESS;
            }

            throw new System.ArgumentException($"Movement type {ownerMovement.GetType()} not accounted for");
        }
    }
}
