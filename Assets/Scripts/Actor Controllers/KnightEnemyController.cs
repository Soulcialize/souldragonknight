using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTrees;

public class KnightEnemyController : EnemyController
{
    [SerializeField] private GroundMovement movement;

    public override Movement Movement { get => movement; }

    protected override BehaviorTreesManager InitializeBehaviorTreesManager()
    {
        return new BehaviorTreesManager(
            new Dictionary<BehaviorTree.Function, BehaviorTree>()
            {
                // construct behavior trees
                {
                    BehaviorTree.Function.COMBAT, CombatTreeConstructor.ConstructGroundCombatTree(movement, combat)
                }
            },
            BehaviorTree.Function.COMBAT);
    }
}
