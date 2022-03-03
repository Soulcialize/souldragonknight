using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTrees;

public class DragonEnemyController : EnemyController
{
    [SerializeField] private AirMovement movement;
    [SerializeField] private ChargeCombat combat;

    public override Movement Movement { get => movement; }
    public override Combat Combat { get => combat; }

    protected override BehaviorTreesManager InitializeBehaviorTreesManager()
    {
        return new BehaviorTreesManager(
            new Dictionary<BehaviorTree.Function, BehaviorTree>()
            {
                // construct behavior trees
                {
                    BehaviorTree.Function.COMBAT, CombatTreeConstructor.ConstructAirCombatTree(movement, combat)
                }
            },
            BehaviorTree.Function.COMBAT);
    }
}
