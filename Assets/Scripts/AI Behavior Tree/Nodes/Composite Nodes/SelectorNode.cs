using System.Collections;
using System.Collections.Generic;
using AiBehaviorTreeNodes;

public class SelectorNode : CompositeNode
{
    public SelectorNode(List<BehaviorNode> children) : base(children) { }

    public override NodeState Execute()
    {
        NodeState result = NodeState.FAILURE;
        foreach (BehaviorNode child in Children)
        {
            result = child.Execute();
            switch (result)
            {
                case NodeState.FAILURE:
                    continue;
                case NodeState.SUCCESS:
                case NodeState.RUNNING:
                    return result;
            }
        }

        return result;
    }
}
