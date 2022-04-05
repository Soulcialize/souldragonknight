using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;
using Pathfinding;

using Filter = System.Predicate<Pathfinding.Node>;

namespace AiBehaviorTreeNodes
{
    public class SetCombatTargetPosNode : BehaviorNode
    {
        private readonly Transform ownerTransform;
        private readonly Movement ownerMovement;
        private readonly Combat ownerCombat;

        private readonly int actorHeight;

        private readonly (List<Filter> hardFilters, List<Filter> softFilters) aerialNavTargetPositionFilters;
        private readonly (List<Filter> hardFilters, List<Filter> softFilters) groundNavTargetPositionFilters;

        public SetCombatTargetPosNode(Movement ownerMovement, Combat ownerCombat)
        {
            ownerTransform = ownerMovement.transform;
            this.ownerMovement = ownerMovement;
            this.ownerCombat = ownerCombat;

            actorHeight = NodeGrid.Instance.GetColliderHeightInNodes(ownerCombat.Collider2d);

            aerialNavTargetPositionFilters = GetAerialNavTargetPositionFilters();
            groundNavTargetPositionFilters = GetGroundNavTargetPositionFilters();

            // factor in collider height when pathfinding
            bool heightFilter(Node node) => NodeGrid.Instance.AreNodesBelowWalkable(node, actorHeight - 1);
            aerialNavTargetPositionFilters.hardFilters.Add(heightFilter);
            groundNavTargetPositionFilters.hardFilters.Add(heightFilter);
        }

        public override NodeState Execute()
        {
            ActorController target = (ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);

            Vector2 navTargetPos = Vector2.zero;
            if (ownerMovement is GroundMovement)
            {
                navTargetPos = Physics2D.Raycast(
                    target.transform.position, Vector2.down, Mathf.Infinity, ownerMovement.GroundDetector.SurfacesLayerMask).point;
                FindGroundPathToCombatTargetPosition(navTargetPos);
            }
            else if (ownerMovement is AirMovement)
            {
                navTargetPos = (Vector2)target.Combat.Collider2d.transform.position + target.Combat.Collider2d.offset;
                FindAerialPathToCombatTargetPosition(navTargetPos);
            }

            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, navTargetPos);
            Blackboard.SetData(
                GeneralBlackboardKeys.NAV_TARGET_STOPPING_DISTANCE,
                Mathf.Min(ownerMovement.DefaultStoppingDistanceFromNavTargets, target.Movement.DefaultStoppingDistanceFromNavTargets));

            return NodeState.SUCCESS;
        }

        private Vector2 GetPathfindingStartPosition()
        {
            return new Vector2(ownerTransform.position.x, ownerCombat.Collider2d.bounds.max.y);
        }

        private void FindAerialPathToCombatTargetPosition(Vector2 targetPosition)
        {
            Blackboard.SetData(
                GeneralBlackboardKeys.NAV_TARGET_PATH,
                Pathfinder.FindPath(
                    NodeGrid.Instance,
                    GetPathfindingStartPosition(),
                    targetPosition + Vector2.up * ownerCombat.Collider2d.bounds.extents.y,
                    aerialNavTargetPositionFilters));
        }

        private void FindGroundPathToCombatTargetPosition(Vector2 targetGroundPosition)
        {
            Blackboard.SetData(
                GeneralBlackboardKeys.NAV_TARGET_PATH,
                Pathfinder.FindPath(
                    NodeGrid.Instance,
                    GetPathfindingStartPosition(),
                    targetGroundPosition + Vector2.up * ownerCombat.Collider2d.bounds.size.y,
                    groundNavTargetPositionFilters));
        }

        private (List<Filter>, List<Filter>) GetAerialNavTargetPositionFilters()
        {
            return (new List<Filter>(), new List<Filter>());
        }

        private (List<Filter>, List<Filter>) GetGroundNavTargetPositionFilters()
        {
            List<Filter> hardFilters = new List<Filter>()
            {
                node => node.DistanceFromSurfaceBelow <= NodeGrid.Instance.NodeDiameter * actorHeight
            };

            return (hardFilters, new List<Filter>());
        }
    }
}
