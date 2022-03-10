using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class StartChargeAttackNode : BehaviorNode
    {
        private readonly Combat ownerCombat;

        public StartChargeAttackNode(Combat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            Transform target = ((GameObject)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET)).transform;
            ownerCombat.ExecuteCombatAbility(CombatAbilityIdentifier.ATTACK_CHARGE, target);
            return NodeState.SUCCESS;
        }
    }
}
