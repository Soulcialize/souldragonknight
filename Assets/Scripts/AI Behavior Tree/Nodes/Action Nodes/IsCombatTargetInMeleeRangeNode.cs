using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class IsCombatTargetInMeleeRangeNode : BehaviorNode
    {
        private readonly Transform ownerTransform;
        private readonly Movement ownerMovement;
        private readonly Combat ownerCombat;

        private readonly bool isBackConsidered;

        /// <param name="ownerMovement">The actor's movement component.</param>
        /// <param name="ownerCombat">The actor's combat component.</param>
        /// <param name="isBackConsidered">Whether to consider the target in melee range if the target is behind the actor.</param>
        public IsCombatTargetInMeleeRangeNode(Movement ownerMovement, Combat ownerCombat, bool isBackConsidered)
        {
            ownerTransform = ownerMovement.transform;
            this.ownerMovement = ownerMovement;
            this.ownerCombat = ownerCombat;
            this.isBackConsidered = isBackConsidered;
        }

        public override NodeState Execute()
        {
            Collider2D targetCollider = ((ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET)).Combat.Collider2d;
            
            if (ownerMovement is GroundMovement groundMovement
                && targetCollider.bounds.center.y > ownerCombat.Collider2d.bounds.max.y + groundMovement.MaxReachableHeight)
            {
                // target is too high above grounded actor
                return NodeState.FAILURE;
            }

            AttackEffectArea attackEffectArea =
                ((MeleeAttackAbility)ownerCombat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_MELEE)).AttackEffectArea;

            // check if target's collider is within attack effect area
            bool isTargetInFrontInMeleeRange
                = IsTargetInMeleeRange(true, ownerMovement.IsFacingRight, ownerTransform, attackEffectArea, targetCollider);
            
            if (!isBackConsidered)
            {
                return isTargetInFrontInMeleeRange ? NodeState.SUCCESS : NodeState.FAILURE;
            }

            return isTargetInFrontInMeleeRange
                || IsTargetInMeleeRange(false, ownerMovement.IsFacingRight, ownerTransform, attackEffectArea, targetCollider)
                ? NodeState.SUCCESS
                : NodeState.FAILURE;
        }

        /// <summary>
        /// Returns true if given target collider is within given attack effect area.
        /// </summary>
        /// <param name="isFrontFacing">Whether to check in front of or behind the actor for the target.</param>
        /// <param name="isFacingRight">Whether the actor is facing right or left.</param>
        /// <param name="attacker">The attacker's transform component.</param>
        /// <param name="attackEffectArea">The attack effect area to use for checking.</param>
        /// <param name="targetCollider">The target collider to check for.</param>
        /// <returns>True if given target collider is within given attack effect area. False otherwise.</returns>
        public static bool IsTargetInMeleeRange(
            bool isFrontFacing, bool isFacingRight, Transform attacker, AttackEffectArea attackEffectArea, Collider2D targetCollider)
        {
            if (!isFrontFacing)
            {
                isFacingRight = !isFacingRight;
            }

            Vector2 minPos = isFacingRight
                ? (Vector2)attacker.position + attackEffectArea.MinLocalPos
                : (Vector2)attacker.position + new Vector2(-attackEffectArea.MaxLocalPos.x, attackEffectArea.MinLocalPos.y);
            Vector2 maxPos = isFacingRight
                ? (Vector2)attacker.position + attackEffectArea.MaxLocalPos
                : (Vector2)attacker.position + new Vector2(-attackEffectArea.MinLocalPos.x, attackEffectArea.MaxLocalPos.y);

            return minPos.x < targetCollider.bounds.max.x
                && targetCollider.bounds.min.x < maxPos.x
                && minPos.y < targetCollider.bounds.max.y
                && targetCollider.bounds.min.y < maxPos.y;
        }
    }
}
