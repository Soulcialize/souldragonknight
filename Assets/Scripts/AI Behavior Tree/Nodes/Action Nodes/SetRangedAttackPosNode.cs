using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;
using Pathfinding;

namespace AiBehaviorTreeNodes
{
    public class SetRangedAttackPosNode : BehaviorNode
    {
        private readonly ActorController owner;

        private readonly Transform projectileOrigin;
        private readonly RangedProjectile projectile;

        private readonly (List<NodeNeighbourFilter> hardFilters, List<NodeNeighbourFilter> softFilters) aerialReadyPositionFilters;
        private readonly (List<NodeNeighbourFilter> hardFilters, List<NodeNeighbourFilter> softFilters) groundReadyPositionFilters;

        public SetRangedAttackPosNode(ActorController owner)
        {
            this.owner = owner;

            RangedAttackAbility rangedAttackAbility = (RangedAttackAbility)owner.Combat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED);
            projectileOrigin = rangedAttackAbility.ProjectileOrigin;
            projectile = rangedAttackAbility.ProjectilePrefab;

            aerialReadyPositionFilters = GetAerialReadyPositionFilters();
            groundReadyPositionFilters = GetGroundReadyPositionFilters();
        }

        public override NodeState Execute()
        {
            ActorController target = (ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);
            Vector3 targetPos = target.transform.position;

            float currDistanceToTarget = Vector2.Distance(projectileOrigin.position, targetPos);
            float maxRange = ((RangedAttackAbility)owner.Combat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED)).MaxRange;

            // use double the height of the projectile so we can ensure we have a clear shot
            RaycastHit2D circleCastToTarget = Physics2D.CircleCast(
                projectileOrigin.position, projectile.GetHeight() * 2f,
                targetPos - projectileOrigin.position, currDistanceToTarget,
                owner.Movement.GroundDetector.SurfacesLayerMask | owner.Combat.AttackEffectLayer);

            if (currDistanceToTarget <= maxRange && circleCastToTarget.collider == target.Combat.Collider2d)
            {
                // in range and can see target; remain at current position
                Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, owner.Pathfinder.GetCurrentPosForPathfinding());
                return NodeState.SUCCESS;
            }

            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, owner.Movement is AirMovement
                ? CalculateAerialReadyPosition(target.Combat.Collider2d.bounds.center)
                : CalculateGroundReadyPosition(target.Combat.Collider2d.bounds.center));

            return NodeState.SUCCESS;
        }

        private Vector2 CalculateAerialReadyPosition(Vector2 targetCenterPos)
        {
            // just keep moving towards target while trying to maintain distance above the ground
            owner.Pathfinder.SetFilters(aerialReadyPositionFilters);
            return targetCenterPos + Vector2.up * owner.Combat.Collider2d.bounds.extents.y;
        }

        private Vector2 CalculateGroundReadyPosition(Vector2 targetCenterPos)
        {
            // head for ground underneath target
            owner.Pathfinder.SetFilters(groundReadyPositionFilters);
            return Physics2D.Raycast(targetCenterPos, Vector2.down, Mathf.Infinity, owner.Movement.GroundDetector.SurfacesLayerMask).point
                + Vector2.up * owner.Combat.Collider2d.bounds.size.y;
        }

        private (List<NodeNeighbourFilter>, List<NodeNeighbourFilter>) GetAerialReadyPositionFilters()
        {
            List<NodeNeighbourFilter> softFilters = new List<NodeNeighbourFilter>()
            {
                new NodeNeighbourFilter((node, neighbour) => neighbour.DistanceFromSurfaceBelow >= 2.5f)
            };

            return (new List<NodeNeighbourFilter>(), softFilters);
        }

        private (List<NodeNeighbourFilter>, List<NodeNeighbourFilter>) GetGroundReadyPositionFilters()
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
