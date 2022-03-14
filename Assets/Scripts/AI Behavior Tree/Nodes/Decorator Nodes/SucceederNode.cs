using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiBehaviorTreeNodes
{
    public class SucceederNode : DecoratorNode
    {
        public SucceederNode(BehaviorNode child) : base(child) { }

        public override NodeState Execute()
        {
            Child.Execute();
            return NodeState.SUCCESS;
        }
    }
}
