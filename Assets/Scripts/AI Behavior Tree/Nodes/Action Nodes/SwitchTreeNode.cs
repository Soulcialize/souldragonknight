using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTrees;

namespace AiBehaviorTreeNodes
{
    public class SwitchTreeNode : BehaviorNode
    {
        private readonly ActorController owner;
        private readonly BehaviorTree.Function tree;

        public SwitchTreeNode(ActorController owner, BehaviorTree.Function tree)
        {
            this.owner = owner;
            this.tree = tree;
        }

        public override NodeState Execute()
        {
            ((EnemyController)owner).BehaviorTreesManager.SwitchActiveTree(tree);
            return NodeState.SUCCESS;
        }
    }
}
