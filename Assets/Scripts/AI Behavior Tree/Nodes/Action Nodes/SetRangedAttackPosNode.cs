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
            float maxRange = ((RangedAttackAbility)ownerCombat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED)).MaxRange;

            if (Vector2.Distance(ownerTransform.position, targetTransform.position) <= maxRange)
            {
                // already in range, remain at current position
                Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, (Vector2)ownerTransform.position);
                return NodeState.SUCCESS;
            }

            if (ownerMovement is AirMovement)
            {
                // move directly towards target
                Vector2 directionToTarget = targetTransform.position - ownerTransform.position;
                Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, (Vector2)targetTransform.position - directionToTarget.normalized * maxRange);
                return NodeState.SUCCESS;
            }

            // attacker is grounded

            // cast ray from target straight down to ground
            RaycastHit2D groundHit = Physics2D.Raycast(targetTransform.position, Vector2.down, maxRange, ownerMovement.GroundDetector.SurfacesLayerMask);
            if (groundHit.distance >= maxRange)
            {
                // impossible to hit even if target is directly above attacker
                return NodeState.FAILURE;
            }

            // get horizontal distance to the point from which an attack can be made
            float horizontalDistance = Mathf.Sqrt(maxRange * maxRange - groundHit.distance * groundHit.distance);
            Vector2 directionToNavTarget = new Vector2(targetTransform.position.x > ownerTransform.position.x ? 1f : -1f, 0f);
            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, (Vector2)ownerTransform.position + directionToNavTarget * horizontalDistance);

            return NodeState.SUCCESS;
        }
    }
}
