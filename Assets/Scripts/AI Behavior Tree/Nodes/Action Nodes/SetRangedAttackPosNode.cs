using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;
using Pathfinding;

using Filter = System.Predicate<Pathfinding.Node>;

namespace AiBehaviorTreeNodes
{
    public class SetRangedAttackPosNode : BehaviorNode
    {
        private readonly Transform ownerTransform;
        private readonly Movement ownerMovement;
        private readonly Combat ownerCombat;

        private readonly int actorHeight;
        private readonly Transform projectileOrigin;
        private readonly RangedProjectile projectile;

        private readonly (List<Filter> hardFilters, List<Filter> softFilters) aerialReadyPositionFilters;
        private readonly (List<Filter> hardFilters, List<Filter> softFilters) groundReadyPositionFilters;

        public SetRangedAttackPosNode(Movement ownerMovement, Combat ownerCombat)
        {
            ownerTransform = ownerMovement.transform;
            this.ownerMovement = ownerMovement;
            this.ownerCombat = ownerCombat;

            actorHeight = NodeGrid.Instance.GetColliderHeightInNodes(ownerCombat.Collider2d);
            RangedAttackAbility rangedAttackAbility = (RangedAttackAbility)ownerCombat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED);
            projectileOrigin = rangedAttackAbility.ProjectileOrigin;
            projectile = rangedAttackAbility.ProjectilePrefab;

            aerialReadyPositionFilters = GetAerialReadyPositionFilters();
            groundReadyPositionFilters = GetGroundReadyPositionFilters();

            // factor in collider height when pathfinding
            bool heightFilter(Node node) => NodeGrid.Instance.AreNodesBelowWalkable(node, actorHeight - 1);
            aerialReadyPositionFilters.hardFilters.Add(heightFilter);
            groundReadyPositionFilters.hardFilters.Add(heightFilter);
        }

        public override NodeState Execute()
        {
            ActorController target = (ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);
            Transform targetTransform = target.transform;

            float currDistanceToTarget = Vector2.Distance(projectileOrigin.position, targetTransform.position);
            float maxRange = ((RangedAttackAbility)ownerCombat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED)).MaxRange;

            RaycastHit2D circleCastToTarget = Physics2D.CircleCast(
                projectileOrigin.position, projectile.GetHeight() / 2f,
                targetTransform.position - projectileOrigin.position, currDistanceToTarget,
                ownerMovement.GroundDetector.SurfacesLayerMask | ownerCombat.AttackEffectLayer);

            if (currDistanceToTarget <= maxRange && circleCastToTarget.collider == target.Combat.Collider2d)
            {
                // in range and can see target; remain at current position
                Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, (Vector2)ownerTransform.position);
                return NodeState.SUCCESS;
            }

            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, ownerMovement is AirMovement
                ? CalculateAerialReadyPosition(targetTransform)
                : CalculateGroundReadyPosition(targetTransform));

            return NodeState.SUCCESS;
        }

        private Vector2 CalculateAerialReadyPosition(Transform target)
        {
            // just keep moving towards target while trying to maintain distance above the ground
            FindPathToAerialReadyPosition(target.position);
            return target.position;
        }

        private Vector2 CalculateGroundReadyPosition(Transform target)
        {
            // head for ground underneath target
            RaycastHit2D groundHit = Physics2D.Raycast(target.position, Vector2.down, Mathf.Infinity, ownerMovement.GroundDetector.SurfacesLayerMask);
            FindPathToGroundReadyPosition(groundHit.point + Vector2.up * ownerCombat.Collider2d.bounds.size.y);
            return groundHit.point;
        }

        private Vector2 GetPathfindingStartPosition()
        {
            return new Vector2(ownerTransform.position.x, ownerCombat.Collider2d.bounds.max.y);
        }

        private void FindPathToAerialReadyPosition(Vector2 navTarget)
        {
            Blackboard.SetData(
                GeneralBlackboardKeys.NAV_TARGET_PATH,
                Pathfinder.FindPath(
                    NodeGrid.Instance,
                    GetPathfindingStartPosition(),
                    navTarget + Vector2.up * ownerCombat.Collider2d.bounds.extents.y,
                    aerialReadyPositionFilters));
        }

        private void FindPathToGroundReadyPosition(Vector2 navTarget)
        {
            Blackboard.SetData(
                GeneralBlackboardKeys.NAV_TARGET_PATH,
                Pathfinder.FindPath(
                    NodeGrid.Instance,
                    GetPathfindingStartPosition(),
                    navTarget,
                    groundReadyPositionFilters));
        }

        private (List<Filter>, List<Filter>) GetAerialReadyPositionFilters()
        {
            List<Filter> softFilters = new List<Filter>()
            {
                node => node.DistanceFromSurfaceBelow >= 3f
            };

            return (new List<Filter>(), softFilters);
        }

        private (List<Filter>, List<Filter>) GetGroundReadyPositionFilters()
        {
            List<Filter> hardFilters = new List<Filter>()
            {
                node => node.DistanceFromSurfaceBelow <= NodeGrid.Instance.NodeDiameter * actorHeight
            };

            return (hardFilters, new List<Filter>());
        }
    }
}
