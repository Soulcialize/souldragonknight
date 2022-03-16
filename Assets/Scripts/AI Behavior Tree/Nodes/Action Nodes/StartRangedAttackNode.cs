using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class StartRangedAttackNode : BehaviorNode
    {
        private readonly Combat ownerCombat;

        public StartRangedAttackNode(Combat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            ActorController target = (ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);
            ownerCombat.ExecuteCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED, target.transform);
            return NodeState.SUCCESS;
        }
    }
}
