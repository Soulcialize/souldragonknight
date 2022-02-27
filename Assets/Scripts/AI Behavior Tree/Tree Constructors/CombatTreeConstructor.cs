using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeNodes;

namespace AiBehaviorTrees
{
    public static class CombatTreeConstructor
    {
        public static BehaviorTree ConstructGroundCombatTree(GroundMovement movement)
        {
            return new BehaviorTree(
                BehaviorTree.Function.COMBAT,
                new SequenceNode(new List<BehaviorNode>()
                {
                    new GetVisibleCombatTargetNode(),
                    new SetCombatTargetPosNode(),
                    new GoToNavTargetNode(movement, true),
                    new StopMovingNode(movement)
                }));
        }
    }
}
