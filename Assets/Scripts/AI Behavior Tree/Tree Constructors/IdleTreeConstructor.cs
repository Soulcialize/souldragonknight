using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;
using AiBehaviorTreeNodes;

namespace AiBehaviorTrees
{
    public static class IdleTreeConstructor
    {
        public static BehaviorTree ConstructIdleTree(ActorController actor, Movement movement, Combat combat, Detection detection)
        {
            return new BehaviorTree(
                new SelectorNode(new List<BehaviorNode>()
                {
                    // check for visible combat target
                    new SequenceNode(new List<BehaviorNode>()
                    {
                        new GetCombatTargetNode(combat, detection, true),
                        new SwitchTreeNode(actor, BehaviorTree.Function.COMBAT)
                    }),
                    // no visible combat target
                    new SequenceNode(new List<BehaviorNode>()
                    {
                        new StopMovingNode(actor),
                        new LookStraightNode(movement, detection)
                    })
                }));
        }
    }
}
