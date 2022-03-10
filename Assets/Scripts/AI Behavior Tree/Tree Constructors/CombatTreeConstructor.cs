using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;
using AiBehaviorTreeNodes;

namespace AiBehaviorTrees
{
    public static class CombatTreeConstructor
    {
        public static BehaviorTree ConstructGroundCombatTree(Movement movement, Combat combat)
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
                            // in ready-attack state
                            new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(ReadyAttackState)),
                            new InverterNode(new SequenceNode(new List<BehaviorNode>()
                            {
                                // exit ready-attack state if target no longer in range
                                new InverterNode(new IsCombatTargetInRangeNode(movement)),
                                new ExitCombatStateMachineNode(combat)
                            }))
                        }),
                        new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(CombatState)),
                        // chasing target
                        new SequenceNode(new List<BehaviorNode>()
                        {
                            new SetCombatTargetPosNode(),
                            new GoToNavTargetNode(movement, true),
                            new StopMovingNode(movement),
                            new FaceNavTargetNode(movement),
                            new StartMeleeAttackNode(combat)
                        })
                    })
                }));
        }

        public static BehaviorTree ConstructAirCombatTree(Movement movement, Combat combat)
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
                            new SequenceNode(new List<BehaviorNode>()
                            {
                                // in ready-attack state
                                new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(ReadyAttackState)),
                                new SelectorNode(new List<BehaviorNode>()
                                {
                                    // turn to face combat target if charge direction not yet locked
                                    new HasLockedTargetPositionNode(combat),
                                    new FaceNavTargetNode(movement)
                                })
                            })
                        }),
                        new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(CombatState)),
                        // chasing target
                        new SequenceNode(new List<BehaviorNode>()
                        {
                            new SetChargeReadyAttackPosNode(combat),
                            new GoToNavTargetNode(movement, false),
                            new StopMovingNode(movement),
                            new StartChargeAttackNode(combat)
                        })
                    })
                }));
        }
    }
}
