using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;
using Pathfinding;

namespace AiBehaviorTreeNodes
{
    public class SetMeleePosNode : BehaviorNode
    {
        private readonly ActorController owner;

        private readonly AttackEffectArea attackEffectArea;

        private readonly (List<NodeNeighbourFilter> hardFilters, List<NodeNeighbourFilter> softFilters) aerialNavTargetPositionFilters;
        private readonly (List<NodeNeighbourFilter> hardFilters, List<NodeNeighbourFilter> softFilters) groundNavTargetPositionFilters;

        public SetMeleePosNode(ActorController owner)
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
                navTargetPos = CalculateGroundReadyPosition(groundMovement, target.Combat.Collider2d);
                owner.Pathfinder.SetFilters(groundNavTargetPositionFilters);
            }
            else if (owner.Movement is AirMovement)
            {
                navTargetPos = CalculateAerialReadyPosition(target.Combat.Collider2d);
                owner.Pathfinder.SetFilters(aerialNavTargetPositionFilters);
            }

            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, navTargetPos);
            return NodeState.SUCCESS;
        }

        private Vector2 CalculateAerialReadyPosition(Collider2D targetCollider)
        {
            return new Vector2(
                targetCollider.bounds.center.x,
                targetCollider.bounds.center.y + owner.Combat.Collider2d.bounds.extents.y);
        }

        private Vector2 CalculateGroundReadyPosition(GroundMovement groundMovement, Collider2D targetCollider)
        {
            if (IsCombatTargetInMeleeRangeNode.IsTargetInMeleeRange(
                true, owner.Movement.IsFacingRight, owner.transform, attackEffectArea, targetCollider))
            {
                // already in melee range, stay at current position
                return owner.Pathfinder.GetCurrentPosForPathfinding();
            }

            // head for ground at target's feet
            Vector2 navTargetPos = Physics2D.Raycast(
                targetCollider.bounds.center, Vector2.down, Mathf.Infinity, owner.Movement.GroundDetector.SurfacesLayerMask).point;

            if (targetCollider.bounds.center.y > groundMovement.MaxReachableHeight)
            {
                // target is too high up to reach
                return navTargetPos + Vector2.up * owner.Combat.Collider2d.bounds.size.y;
            }

            // offset navigation target position to a spot next to the target, leaving the target in melee range
            return new Vector2(
                navTargetPos.x + (owner.Movement.IsFacingRight ? -attackEffectArea.LocalPos.x : attackEffectArea.LocalPos.x),
                navTargetPos.y + owner.Combat.Collider2d.bounds.size.y);
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
