using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Action node that sets the position at which to ready an attack as the navigation target.
    /// </summary>
    /// <remarks>
    /// <br><b>Success</b>: Always.</br>
    /// <br><b>Failure</b>: -</br>
    /// <br><b>Running</b>: -</br>
    /// </remarks>
    public class SetChargeReadyAttackPosNode : BehaviorNode
    {
        private readonly Combat ownerCombat;
        private readonly ChargeAttackAbility chargeAttackAbility;

        public SetChargeReadyAttackPosNode(Combat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
            chargeAttackAbility = (ChargeAttackAbility)ownerCombat.GetCombatAbility(CombatAbilityIdentifier.ATTACK_CHARGE);
        }

        public override NodeState Execute()
        {
            Vector2 targetPos = ((GameObject)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET)).transform.position;
            Vector2 currentPos = ownerCombat.transform.position;

            Vector2 readyAttackPos;
            if (Vector2.Distance(currentPos, targetPos) <= chargeAttackAbility.ReadyAttackDistance)
            {
                readyAttackPos = currentPos;
            }
            else
            {
                Vector2 directionToTarget = (targetPos - currentPos).normalized;
                readyAttackPos = targetPos + -directionToTarget * chargeAttackAbility.ReadyAttackDistance;
            }

            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, readyAttackPos);
            return NodeState.SUCCESS;
        }
    }
}
