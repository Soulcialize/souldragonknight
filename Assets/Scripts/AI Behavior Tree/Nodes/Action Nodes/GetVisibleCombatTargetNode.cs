using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Action node that gets the nearest visible combat target and stores it in the blackboard.
    /// </summary>
    /// <remarks>
    /// <br><b>Success</b>: Visible target found and stored in the blackboard.</br>
    /// <br><b>Failure</b>: No visible target found.</br>
    /// <br><b>Running</b>: -</br>
    /// </remarks>
    public class GetVisibleCombatTargetNode : BehaviorNode
    {
        private readonly Combat ownerCombat;

        public GetVisibleCombatTargetNode(Combat ownerCombat)
        {
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            // TODO: use viewcone detection instead of radial grab
            Collider2D targetCollider = Physics2D.OverlapCircle(ownerCombat.transform.position, 100f, ownerCombat.AttackEffectLayer);
            Blackboard.SetData(CombatBlackboardKeys.COMBAT_TARGET, targetCollider.gameObject);
            return NodeState.SUCCESS;
        }
    }
}
