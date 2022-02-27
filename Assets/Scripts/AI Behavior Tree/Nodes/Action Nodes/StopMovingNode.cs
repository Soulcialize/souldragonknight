using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiBehaviorTreeNodes
{
    public class StopMovingNode : MovementNode
    {
        public StopMovingNode(Movement ownerMovement) : base(ownerMovement) { }

        public override NodeState Execute()
        {
            UpdateMovement(Vector2.zero);
            return NodeState.SUCCESS;
        }
    }
}
