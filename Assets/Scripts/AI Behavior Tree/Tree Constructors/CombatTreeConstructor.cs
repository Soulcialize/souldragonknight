using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;
using AiBehaviorTreeNodes;

namespace AiBehaviorTrees
{
    public static class CombatTreeConstructor
    {
        public static BehaviorTree ConstructMeleeCombatTree(Movement movement, Combat combat)
        {
            return new BehaviorTree(
                new GetVisibleCombatTargetNode(combat),
                new SequenceNode(new List<BehaviorNode>()
                {
                    // get visible combat target
                    new SelectorNode(new List<BehaviorNode>()
                    {
                        new GetVisibleCombatTargetNode(combat),
                        // failed to get visible combat target; fail out of tree
                        new InverterNode(new SucceederNode(new SequenceNode(new List<BehaviorNode>()
                        {
                            // exit state machine if readying attack
                            new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(ReadyAttackState)),
                            new ExitCombatStateMachineNode(combat)
                        })))
                    }),
                    // found visible combat target
                    new SelectorNode(new List<BehaviorNode>()
                    {
                        // in combat state, engaging target
                        new SequenceNode(new List<BehaviorNode>()
                        {
                            // in ready-attack state
                            new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(ReadyAttackState)),
                            new InverterNode(new SequenceNode(new List<BehaviorNode>()
                            {
                                // exit ready-attack state if target is no longer in range
                                new InverterNode(new IsCombatTargetInMeleeRangeNode(movement)),
                                new ExitCombatStateMachineNode(combat)
                            }))
                        }),
                        new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(CombatState)),
                        // chasing target
                        new SequenceNode(new List<BehaviorNode>()
                        {
                            new SetCombatTargetPosNode(movement),
                            new SelectorNode(new List<BehaviorNode>()
                            {
                                new SequenceNode(new List<BehaviorNode>()
                                {
                                    // can reach target
                                    new CanReachNavTargetNode(movement),
                                    new GoToNavTargetNode(movement, true),
                                    new StopMovingNode(movement),
                                    new FaceNavTargetNode(movement),
                                    new StartMeleeAttackNode(combat)
                                }),
                                new SequenceNode(new List<BehaviorNode>()
                                {
                                    // cannot reach target
                                    new StopMovingNode(movement),
                                    new FaceNavTargetNode(movement)
                                })
                            })
                        })
                    })
                }));
        }

        public static BehaviorTree ConstructRangedCombatTree(Movement movement, Combat combat)
        {
            return new BehaviorTree(
                new SequenceNode(new List<BehaviorNode>()
                {
                    new GetVisibleCombatTargetNode(combat),
                    new AddListenerToCombatTargetRangedAttackNode(combat)
                }),
                new SequenceNode(new List<BehaviorNode>()
                {
                    // get visible combat target
                    new SelectorNode(new List<BehaviorNode>()
                    {
                        new GetVisibleCombatTargetNode(combat),
                        // failed to get visible combat target; fail out of tree
                        new InverterNode(new SucceederNode(new SequenceNode(new List<BehaviorNode>()
                        {
                            // exit state machine if readying attack
                            new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(ReadyAttackState)),
                            new ExitCombatStateMachineNode(combat)
                        })))
                    }),
                    // found visible combat target
                    new SelectorNode(new List<BehaviorNode>()
                    {
                        // in combat state, engaging target
                        new SequenceNode(new List<BehaviorNode>()
                        {
                            // in ready-attack state
                            new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(ReadyRangedAttackState)),
                            // face target while readying if target position not locked yet
                            new InverterNode(new HasLockedTargetPositionNode(combat)),
                            new SetCombatTargetPosNode(movement),
                            new FaceNavTargetNode(movement)
                        }),
                        new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(CombatState)),
                        // chasing target
                        new SequenceNode(new List<BehaviorNode>()
                        {
                            new SetRangedAttackPosNode(movement, combat),
                            new SelectorNode(new List<BehaviorNode>()
                            {
                                new SequenceNode(new List<BehaviorNode>()
                                {
                                    // can reach target
                                    new CanReachNavTargetNode(movement),
                                    new GoToNavTargetNode(movement, false),
                                    new StopMovingNode(movement),
                                    new StartRangedAttackNode(combat)
                                }),
                                new SequenceNode(new List<BehaviorNode>()
                                {
                                    // cannot reach target
                                    new StopMovingNode(movement),
                                    new FaceNavTargetNode(movement)
                                })
                            })
                        })
                    })
                }));
        }
    }
}
