using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Node that handles the addition/removal of a listener to/from the combat target's ranged attack.
    /// </summary>
    public class ManageCombatTargetRangedAttackListenerNode : BehaviorNode
    {
        private readonly Combat ownerCombat;

        // we're assuming that the combat target will always be the same actor
        private RangedAttackAbility combatTargetRangedAttackAbility;

        public ManageCombatTargetRangedAttackListenerNode(Combat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            object targetObj = Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);
            if (targetObj == null)
            {
                if (combatTargetRangedAttackAbility != null)
                {
                    combatTargetRangedAttackAbility.FireRangedProjectileEvent.RemoveListener(ownerCombat.OnProjectileFiredEvent);
                    combatTargetRangedAttackAbility = null;
                }

                return NodeState.FAILURE;
            }
            
            if (combatTargetRangedAttackAbility != null)
            {
                // already added to combat target
                return NodeState.SUCCESS;
            }

            ActorController target = (ActorController)targetObj;
            if (!target.Combat.HasCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED))
            {
                return NodeState.FAILURE;
            }

            combatTargetRangedAttackAbility = (RangedAttackAbility)target.Combat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED);
            combatTargetRangedAttackAbility.FireRangedProjectileEvent.AddListener(ownerCombat.OnProjectileFiredEvent);
            return NodeState.SUCCESS;
        }
    }
}
