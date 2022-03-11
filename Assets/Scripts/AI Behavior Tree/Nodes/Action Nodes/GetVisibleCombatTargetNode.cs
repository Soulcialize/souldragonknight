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
            Collider2D targetCollider = Physics2D.OverlapCircle(ownerCombat.transform.position, 1000f, ownerCombat.AttackEffectLayer);
            if (targetCollider != null && !targetCollider.GetComponentInParent<Health>().IsZero())
            {
                Blackboard.SetData(CombatBlackboardKeys.COMBAT_TARGET, targetCollider.gameObject);
                return NodeState.SUCCESS;
            } 
            else
            {
                return NodeState.FAILURE;
            }
        }
    }
}
