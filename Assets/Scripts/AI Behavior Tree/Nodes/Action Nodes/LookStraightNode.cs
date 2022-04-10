using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiBehaviorTreeNodes
{
    public class LookStraightNode : BehaviorNode
    {
        private readonly Movement ownerMovement;
        private readonly Detection ownerDetection;

        public LookStraightNode(Movement ownerMovement, Detection ownerDetection)
        {
            this.ownerMovement = ownerMovement;
            this.ownerDetection = ownerDetection;
        }

        public override NodeState Execute()
        {
            ownerDetection.LookStraight(ownerMovement.IsFacingRight);
            return NodeState.SUCCESS;
        }
    }
}
