using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class AddListenerToCombatTargetRangedAttackNode : BehaviorNode
    {
        private readonly Combat ownerCombat;

        public AddListenerToCombatTargetRangedAttackNode(Combat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            ActorController target = (ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);
            if (!target.Combat.HasCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED))
            {
                return NodeState.FAILURE;
            }

            RangedAttackAbility targetAbility = (RangedAttackAbility)target.Combat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED);
            targetAbility.FireRangedProjectileEvent.AddListener(ownerCombat.OnProjectileFiredEvent);
            return NodeState.SUCCESS;
        }
    }
}
