using System.Collections;
using System.Collections.Generic;
using AiBehaviorTreeNodes;

public class SequenceNode : CompositeNode
{
    public SequenceNode(List<BehaviorNode> children) : base(children) { }

    public override NodeState Execute()
    {
        NodeState result = NodeState.SUCCESS;
        foreach (BehaviorNode child in Children)
        {
            result = child.Execute();
            switch (result)
            {
                case NodeState.SUCCESS:
                    continue;
                case NodeState.FAILURE:
                case NodeState.RUNNING:
                    return result;
            }
        }

        return result;
    }
}
