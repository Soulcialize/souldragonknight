using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Blackboard = AiBehaviorTreeBlackboards.Blackboard;

namespace AiBehaviorTreeNodes
{
    public abstract class BehaviorNode
    {
        public enum NodeState { FAILURE, SUCCESS, RUNNING }

        public BehaviorNode Parent { get; set; }

        public Blackboard Blackboard { get; set; }

        public abstract NodeState Execute();
    }
}
