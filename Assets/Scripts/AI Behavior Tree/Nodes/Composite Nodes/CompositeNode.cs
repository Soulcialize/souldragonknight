using System.Collections;
using System.Collections.Generic;

namespace AiBehaviorTreeNodes
{
    public abstract class CompositeNode : BehaviorNode
    {
        public List<BehaviorNode> Children { get; protected set; }

        public CompositeNode(List<BehaviorNode> children)
        {
            Children = children;
            foreach (BehaviorNode child in children)
            {
                child.Parent = this;
            }
        }
    }
}
