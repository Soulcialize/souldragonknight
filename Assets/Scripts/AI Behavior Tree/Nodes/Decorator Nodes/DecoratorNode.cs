using System.Collections;
using System.Collections.Generic;

namespace AiBehaviorTreeNodes
{
    public abstract class DecoratorNode : BehaviorNode
    {
        public BehaviorNode Child;

        public DecoratorNode(BehaviorNode child)
        {
            Child = child;
            child.Parent = this;
        }
    }
}
