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

        private readonly AttackEffectArea attackEffectArea;

        private readonly (List<NodeNeighbourFilter> hardFilters, List<NodeNeighbourFilter> softFilters) aerialNavTargetPositionFilters;
        private readonly (List<NodeNeighbourFilter> hardFilters, List<NodeNeighbourFilter> softFilters) groundNavTargetPositionFilters;

        public SetCombatTargetPosNode(ActorController owner)
        {
            this.owner = owner;

            attackEffectArea = ((MeleeAttackAbility)owner.Combat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_MELEE)).AttackEffectArea;

            aerialNavTargetPositionFilters = GetAerialNavTargetPositionFilters();
            groundNavTargetPositionFilters = GetGroundNavTargetPositionFilters();
        }

        public override NodeState Execute()
        {
            ActorController target = (ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);

            Vector2 navTargetPos = Vector2.zero;
            if (owner.Movement is GroundMovement groundMovement)
            {
                navTargetPos = Physics2D.Raycast(
                    target.Combat.Collider2d.bounds.center, Vector2.down, Mathf.Infinity, owner.Movement.GroundDetector.SurfacesLayerMask).point;
                if (target.Combat.Collider2d.bounds.center.y > groundMovement.MaxReachableHeight)
                {
                    // target is too high up to reach
                    navTargetPos += Vector2.up * owner.Combat.Collider2d.bounds.size.y;
                }
                else
                {
                    navTargetPos = new Vector2(
                        navTargetPos.x + (owner.Movement.IsFacingRight ? -attackEffectArea.LocalPos.x : attackEffectArea.LocalPos.x),
                        navTargetPos.y + owner.Combat.Collider2d.bounds.size.y);
                }

                owner.Pathfinder.SetFilters(groundNavTargetPositionFilters);
            }
            else if (owner.Movement is AirMovement)
            {
                navTargetPos = new Vector2(
                    target.Combat.Collider2d.bounds.center.x,
                    target.Combat.Collider2d.bounds.center.y + owner.Combat.Collider2d.bounds.extents.y);
                owner.Pathfinder.SetFilters(aerialNavTargetPositionFilters);
            }

            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, navTargetPos);
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
