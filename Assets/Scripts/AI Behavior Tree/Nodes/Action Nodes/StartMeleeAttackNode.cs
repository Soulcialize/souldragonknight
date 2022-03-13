using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiBehaviorTreeNodes
{
    public class StartMeleeAttackNode : BehaviorNode
    {
        private readonly Combat ownerCombat;

        public StartMeleeAttackNode(Combat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            ownerCombat.ExecuteCombatAbility(CombatAbilityIdentifier.ATTACK_MELEE);
            return NodeState.SUCCESS;
        }
    }
}
