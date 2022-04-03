using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class SetRangedAttackPosNode : BehaviorNode
    {
        private readonly Transform ownerTransform;
        private readonly Movement ownerMovement;
        private readonly Combat ownerCombat;

        public SetRangedAttackPosNode(Movement ownerMovement, Combat ownerCombat)
        {
            ownerTransform = ownerMovement.transform;
            this.ownerMovement = ownerMovement;
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            ActorController target = (ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);
            Transform targetTransform = target.transform;

            float currDistanceToTarget = Vector2.Distance(ownerTransform.position, targetTransform.position);
            float maxRange = ((RangedAttackAbility)ownerCombat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED)).MaxRange;

            RaycastHit2D raycastToTarget = Physics2D.Raycast(
                ownerTransform.position, targetTransform.position - ownerTransform.position, currDistanceToTarget,
                ownerMovement.GroundDetector.SurfacesLayerMask | ownerCombat.AttackEffectLayer);

            if (currDistanceToTarget <= maxRange && raycastToTarget.collider == target.Combat.Collider2d)
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
            FindPathToAerialReadyPosition(target.position, 3f);
            return target.position;
        }

        private Vector2 CalculateGroundReadyPosition(Transform target)
        {
            // head for ground underneath target
            RaycastHit2D groundHit = Physics2D.Raycast(target.position, Vector2.down, Mathf.Infinity, ownerMovement.GroundDetector.SurfacesLayerMask);
            FindPathToGroundReadyPosition(groundHit.point + Vector2.up * ownerCombat.Collider2d.bounds.extents.y);
            return groundHit.point;
        }

        private void FindPathToAerialReadyPosition(Vector2 navTarget, float minHeightAboveGround)
        {
            Blackboard.SetData(
                GeneralBlackboardKeys.NAV_TARGET_PATH,
                Pathfinding.Pathfinder.FindPath(
                    Pathfinding.NodeGrid.Instance,
                    ownerTransform.position,
                    navTarget,
                    false,
                    node => node.DistanceFromSurfaceBelow >= minHeightAboveGround));
        }

        private void FindPathToGroundReadyPosition(Vector2 navTarget)
        {
            RaycastHit2D groundHit = Physics2D.Raycast(ownerTransform.position, Vector2.down, Mathf.Infinity, ownerMovement.GroundDetector.SurfacesLayerMask);
            float maxDistanceFromGround = groundHit.distance + ((GroundMovement)ownerMovement).MaxReachableHeight;
            Blackboard.SetData(
                GeneralBlackboardKeys.NAV_TARGET_PATH,
                Pathfinding.Pathfinder.FindPath(
                    Pathfinding.NodeGrid.Instance,
                    ownerTransform.position,
                    navTarget,
                    true,
                    node => node.DistanceFromSurfaceBelow <= maxDistanceFromGround));
        }
    }
}
