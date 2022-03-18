using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class IsCombatTargetInMeleeRangeNode : BehaviorNode
    {
        private readonly Transform ownerTransform;
        private readonly Combat ownerCombat;
        
        public IsCombatTargetInMeleeRangeNode(Combat ownerCombat)
        {
            ownerTransform = ownerCombat.transform;
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            Vector2 targetPosition = ((ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET)).transform.position;
            float maxMeleeRange = ((MeleeAttackAbility)ownerCombat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_MELEE)).MaximumReach;
            return Vector2.Distance(ownerTransform.position, targetPosition) <= maxMeleeRange
                ? NodeState.SUCCESS
                : NodeState.FAILURE;
        }
    }
}
