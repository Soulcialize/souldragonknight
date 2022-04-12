using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Action node that gets a combat target in range and stores it in the blackboard.
    /// </summary>
    /// <remarks>
    /// <br><b>Success</b>: Target found and stored in the blackboard.</br>
    /// <br><b>Failure</b>: No target found.</br>
    /// <br><b>Running</b>: -</br>
    /// </remarks>
    public class GetCombatTargetNode : BehaviorNode
    {
        private readonly Transform ownerTransform;
        private readonly Combat ownerCombat;
        private readonly Detection ownerDetection;

        private readonly bool isVisibilityNeeded;

        /// <param name="ownerCombat">The owning actor's combat component.</param>
        /// <param name="ownerDetection">The owning actor's detection component.</param>
        /// <param name="isVisibilityNeeded">Whether the target needs to be visible.</param>
        public GetCombatTargetNode(Combat ownerCombat, Detection ownerDetection, bool isVisibilityNeeded)
        {
            ownerTransform = ownerDetection.transform;
            this.ownerCombat = ownerCombat;
            this.ownerDetection = ownerDetection;
            this.isVisibilityNeeded = isVisibilityNeeded;
        }

        public override NodeState Execute()
        {
            object currentTargetObj = Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);
            Collider2D targetCollider = Physics2D.OverlapCircle(ownerTransform.position, ownerDetection.ViewDistance, ownerCombat.AttackEffectLayer);
            if (targetCollider == null || isVisibilityNeeded && !ownerDetection.IsVisible(targetCollider))
            {
                if (currentTargetObj != null)
                {
                    ownerDetection.CombatTargetLostEvent.Invoke((ActorController)currentTargetObj);
                }

                Blackboard.SetData(CombatBlackboardKeys.COMBAT_TARGET, null);
                return NodeState.FAILURE;
            }

            ActorController target = ActorController.GetActorFromCollider(targetCollider);
            if (currentTargetObj == null || (ActorController)currentTargetObj != target)
            {
                Blackboard.SetData(CombatBlackboardKeys.COMBAT_TARGET, target);
                ownerDetection.CombatTargetDetectedEvent.Invoke(target);
            }

            return NodeState.SUCCESS;
        }
    }
}
