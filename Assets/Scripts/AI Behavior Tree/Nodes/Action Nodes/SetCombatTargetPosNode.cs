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
        private readonly ActorController owner;

        private readonly (List<Filter> hardFilters, List<Filter> softFilters) aerialNavTargetPositionFilters;
        private readonly (List<Filter> hardFilters, List<Filter> softFilters) groundNavTargetPositionFilters;

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
                    target.transform.position, Vector2.down, Mathf.Infinity, owner.Movement.GroundDetector.SurfacesLayerMask).point;
                navTargetPos += Vector2.up * owner.Combat.Collider2d.bounds.size.y;
                owner.Pathfinder.SetFilters(groundNavTargetPositionFilters);
            }
            else if (owner.Movement is AirMovement)
            {
                navTargetPos = (Vector2)target.Combat.Collider2d.transform.position + target.Combat.Collider2d.offset;
                navTargetPos += Vector2.up * owner.Combat.Collider2d.bounds.extents.y;
                owner.Pathfinder.SetFilters(aerialNavTargetPositionFilters);
            }

            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, navTargetPos);
            Blackboard.SetData(
                GeneralBlackboardKeys.NAV_TARGET_STOPPING_DISTANCE,
                Mathf.Min(owner.Movement.DefaultStoppingDistanceFromNavTargets, target.Movement.DefaultStoppingDistanceFromNavTargets));

            return NodeState.SUCCESS;
        }

        private (List<Filter>, List<Filter>) GetAerialNavTargetPositionFilters()
        {
            return (new List<Filter>(), new List<Filter>());
        }

        private (List<Filter>, List<Filter>) GetGroundNavTargetPositionFilters()
        {
            List<Filter> hardFilters = new List<Filter>()
            {
                node => node.DistanceFromSurfaceBelow <= NodeGrid.Instance.NodeDiameter * owner.Pathfinder.HeightInNodes
            };

            return (hardFilters, new List<Filter>());
        }
    }
}
