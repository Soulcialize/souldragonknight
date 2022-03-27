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
            Transform target = ((ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET)).transform;

            float currDistanceToTarget = Vector2.Distance(ownerTransform.position, target.position);
            float maxRange = ((RangedAttackAbility)ownerCombat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED)).MaxRange;

            if (currDistanceToTarget <= maxRange)
            {
                // already in range, remain at current position
                Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, (Vector2)ownerTransform.position);
                return NodeState.SUCCESS;
            }

            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, ownerMovement is AirMovement
                ? CalculateAerialReadyPosition(target)
                : CalculateGroundReadyPosition(target, maxRange));

            return NodeState.SUCCESS;
        }

        private Vector2 CalculateAerialReadyPosition(Transform target)
        {
            // just keep moving towards target on the horizontal axis
            Vector2 directionToNavTarget = new Vector2(target.position.x > ownerTransform.position.x ? 1f : -1f, 0f);
            return (Vector2)ownerTransform.position + directionToNavTarget;
        }

        private Vector2 CalculateGroundReadyPosition(Transform target, float maxRange)
        {
            // cast ray from target straight down to ground
            RaycastHit2D groundHit = Physics2D.Raycast(target.position, Vector2.down, maxRange, ownerMovement.GroundDetector.SurfacesLayerMask);
            if (groundHit.distance >= maxRange)
            {
                // impossible to hit even if target is directly above attacker; return point under target for now
                return groundHit.point;
            }

            // get horizontal distance to the point from which an attack can be made
            float horizontalDistance = Mathf.Sqrt(maxRange * maxRange - groundHit.distance * groundHit.distance);
            Vector2 directionToNavTarget = new Vector2(target.position.x > ownerTransform.position.x ? 1f : -1f, 0f);
            return (Vector2)ownerTransform.position + directionToNavTarget * horizontalDistance;
        }
    }
}
