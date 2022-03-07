using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    public class StartClashNode : BehaviorNode
    {
        private readonly Movement ownerMovement;
        private readonly MeleeCombat ownerCombat;

        public StartClashNode(Movement ownerMovement, MeleeCombat ownerCombat)
        {
            this.ownerMovement = ownerMovement;
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            MeleeCombat targetCombat = ((GameObject)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET)).GetComponent<MeleeCombat>();
            if (targetCombat == null)
            {
                return NodeState.FAILURE;
            }

            Vector2 ownerKnockbackDirection = new Vector2(ownerMovement.IsFacingRight ? -1f : 1f, 0f);
            ownerCombat.Clash(ownerKnockbackDirection);

            targetCombat.GetComponent<Movement>().UpdateMovement(Vector2.zero);
            targetCombat.Clash(-ownerKnockbackDirection);
            
            return NodeState.SUCCESS;
        }
    }
}
