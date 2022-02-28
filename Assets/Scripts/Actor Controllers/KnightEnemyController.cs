using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTrees;

public class KnightEnemyController : EnemyController
{
    [SerializeField] private GroundMovement movement;

    public BehaviorTreesManager BehaviorTreesManager { get; private set; }

    private void Awake()
    {
        BehaviorTreesManager = new BehaviorTreesManager(
            new Dictionary<BehaviorTree.Function, BehaviorTree>()
            {
                // construct behavior trees
                {
                    BehaviorTree.Function.COMBAT, CombatTreeConstructor.ConstructGroundCombatTree(movement, combat)
                }
            },
            BehaviorTree.Function.COMBAT);
    }

    private void Update()
    {
        BehaviorTreesManager.UpdateActiveTree();
    }
}
