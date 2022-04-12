using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Action node that checks for a visible combat target.
    /// </summary>
    /// <remarks>
    /// <br><b>Success</b>: Target found.</br>
    /// <br><b>Failure</b>: No target found.</br>
    /// <br><b>Running</b>: -</br>
    /// </remarks>
    public class CheckForVisibleCombatTargetNode : BehaviorNode
    {
        private readonly Transform ownerTransform;
        private readonly Combat ownerCombat;
        private readonly Detection ownerDetection;

        public CheckForVisibleCombatTargetNode(Combat ownerCombat, Detection ownerDetection)
        {
            ownerTransform = ownerDetection.transform;
            this.ownerCombat = ownerCombat;
            this.ownerDetection = ownerDetection;
        }

        public override NodeState Execute()
        {
            Collider2D targetCollider = Physics2D.OverlapCircle(ownerTransform.position, ownerDetection.ViewDistance, ownerCombat.AttackEffectLayer);
            return targetCollider != null && ownerDetection.IsVisible(targetCollider)
                ? NodeState.SUCCESS
                : NodeState.FAILURE;
        }
    }
}
