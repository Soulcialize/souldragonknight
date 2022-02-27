using System.Collections;
using System.Collections.Generic;

namespace AiBehaviorTreeNodes
{
    public class InverterNode : DecoratorNode
    {
        public InverterNode(BehaviorNode child) : base(child) { }

        public override NodeState Execute()
        {
            NodeState result = Child.Execute();
            switch (result)
            {
                case NodeState.SUCCESS:
                    return NodeState.FAILURE;
                case NodeState.FAILURE:
                    return NodeState.SUCCESS;
                case NodeState.RUNNING:
                    return result;
                default:
                    throw new System.ArgumentException($"Child node does not return a state that can be inverted");
            }
        }
    }
}
