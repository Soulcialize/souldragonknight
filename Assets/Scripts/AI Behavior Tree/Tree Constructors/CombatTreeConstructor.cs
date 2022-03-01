using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;
using AiBehaviorTreeNodes;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTrees
{
    public static class CombatTreeConstructor
    {
        public static BehaviorTree ConstructGroundCombatTree(Movement movement, MeleeCombat combat)
        {
            return new BehaviorTree(
                BehaviorTree.Function.COMBAT,
                new SequenceNode(new List<BehaviorNode>()
                {
                    new GetVisibleCombatTargetNode(combat),
                    new UntilFailNode(new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(CombatState))),
                    new SetCombatTargetPosNode(),
                    new GoToNavTargetNode(movement, true),
                    new StopMovingNode(movement),
                    new FaceNavTargetNode(movement),
                    new ReadyAttackNode(combat)
                }));
        }

        public static BehaviorTree ConstructAirCombatTree(Movement movement, TouchCombat combat)
        {
            return new BehaviorTree(
                BehaviorTree.Function.COMBAT,
                new SequenceNode(new List<BehaviorNode>()
                {
                    new GetVisibleCombatTargetNode(combat),
                    new SelectorNode(new List<BehaviorNode>()
                    {
                        // in combat state, engaging target
                        new SequenceNode(new List<BehaviorNode>()
                        {
                            new SetCombatTargetPosNode(),
                            new SelectorNode(new List<BehaviorNode>()
                            {
                                new SequenceNode(new List<BehaviorNode>()
                                {
                                    new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(ReadyAttackState)),
                                    new FaceNavTargetNode(movement)
                                }),
                                new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(CombatState)),
                            })
                        }),
                        // chasing target
                        new SequenceNode(new List<BehaviorNode>()
                        {
                            new SetReadyAttackPosNode(combat),
                            new GoToNavTargetNode(movement, false),
                            new StopMovingNode(movement),
                            new ReadyAttackNode(combat)
                        })
                    })
                }));
        }
    }
}
