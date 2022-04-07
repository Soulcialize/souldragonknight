using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;
using Pathfinding;

namespace AiBehaviorTreeNodes
{
    public class SetCombatTargetPosNode : BehaviorNode
    {
        private readonly ActorController owner;

        private readonly (List<NodeNeighbourFilter> hardFilters, List<NodeNeighbourFilter> softFilters) aerialNavTargetPositionFilters;
        private readonly (List<NodeNeighbourFilter> hardFilters, List<NodeNeighbourFilter> softFilters) groundNavTargetPositionFilters;

        public SetCombatTargetPosNode(ActorController owner)
        {
            this.owner = owner;

            aerialNavTargetPositionFilters = GetAerialNavTargetPositionFilters();
            groundNavTargetPositionFilters = GetGroundNavTargetPositionFilters();
        }

        public override NodeState Execute()
        {
            ActorController target = (ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);

            Vector2 navTargetPos = Vector2.zero;
            if (owner.Movement is GroundMovement)
            {
                navTargetPos = Physics2D.Raycast(
                    target.Combat.Collider2d.bounds.center, Vector2.down, Mathf.Infinity, owner.Movement.GroundDetector.SurfacesLayerMask).point;
                navTargetPos += Vector2.up * owner.Combat.Collider2d.bounds.size.y;
                owner.Pathfinder.SetFilters(groundNavTargetPositionFilters);
            }
            else if (owner.Movement is AirMovement)
            {
                navTargetPos = (Vector2)target.Combat.Collider2d.bounds.center + target.Combat.Collider2d.offset;
                navTargetPos += Vector2.up * owner.Combat.Collider2d.bounds.extents.y;
                owner.Pathfinder.SetFilters(aerialNavTargetPositionFilters);
            }

            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, navTargetPos);
            Blackboard.SetData(
                GeneralBlackboardKeys.NAV_TARGET_STOPPING_DISTANCE,
                Mathf.Max(owner.Movement.DefaultStoppingDistanceFromNavTargets, target.Movement.DefaultStoppingDistanceFromNavTargets));

            return NodeState.SUCCESS;
        }

        private (List<NodeNeighbourFilter>, List<NodeNeighbourFilter>) GetAerialNavTargetPositionFilters()
        {
            return (new List<NodeNeighbourFilter>(), new List<NodeNeighbourFilter>());
        }

        private (List<NodeNeighbourFilter>, List<NodeNeighbourFilter>) GetGroundNavTargetPositionFilters()
        {
            List<NodeNeighbourFilter> hardFilters = new List<NodeNeighbourFilter>()
            {
                new NodeNeighbourFilter((node, neighbour) =>
                {
                    // make sure the path only involves jumping up a step or going off a drop
                    return neighbour.GridY <= node.GridY
                        || neighbour.DistanceFromSurfaceBelow <= NodeGrid.Instance.NodeDiameter * owner.Pathfinder.HeightInNodes;
                })
            };

            return (hardFilters, new List<NodeNeighbourFilter>());
        }
    }
}
