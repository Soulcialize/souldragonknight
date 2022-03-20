using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeNodes;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTrees
{
    public class BehaviorTree
    {
        public enum Function
        {
            NONE,
            IDLE,
            COMBAT
        }

        private BehaviorNode entry;
        private BehaviorNode update;

        public BehaviorTree(BehaviorNode update)
        {
            this.update = update;

            Blackboard.CreateBlackboardForTree(update);
        }

        public BehaviorTree(BehaviorNode entry, BehaviorNode update)
        {
            this.entry = entry;
            this.update = update;

            // create a blackboard that is shared between the entry and update subtrees
            Blackboard.CreateBlackboardForTree(new SequenceNode(new List<BehaviorNode>() { entry, update }));
        }

        public BehaviorNode.NodeState Enter()
        {
            if (entry == null)
            {
                return BehaviorNode.NodeState.SUCCESS;
            }

            return entry.Execute();
        }

        public BehaviorNode.NodeState Update()
        {
            return update.Execute();
        }

        public void Exit()
        {
            update.Blackboard.ClearAllData();
        }
    }
}
