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
                                new InverterNode(new IsCombatTargetInMeleeRangeNode(movement)),
                                new ExitCombatStateMachineNode(combat)
                            }))
                        }),
                        new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(CombatState)),
                        // chasing target
                        new SequenceNode(new List<BehaviorNode>()
                        {
                            new SetCombatTargetPosNode(movement),
                            new GoToNavTargetNode(movement, true),
                            new StopMovingNode(movement),
                            new FaceNavTargetNode(movement),
                            new StartMeleeAttackNode(combat)
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
                    new GetVisibleCombatTargetNode(combat),
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
                            new GoToNavTargetNode(movement, false),
                            new StopMovingNode(movement),
                            new StartRangedAttackNode(combat)
                        })
                    })
                }));
        }
    }
}
