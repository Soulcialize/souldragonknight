using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTrees;

public class RangedKnightEnemyController : EnemyController
{
    [SerializeField] private AirMovement movement;

    public override Movement Movement { get => movement; }

    protected override BehaviorTreesManager InitializeBehaviorTreesManager()
    {
        return new BehaviorTreesManager(
            new Dictionary<BehaviorTree.Function, BehaviorTree>()
            {
                {
                    BehaviorTree.Function.COMBAT, CombatTreeConstructor.ConstructRangedCombatTree(movement, combat)
                }
            },
            BehaviorTree.Function.COMBAT);
    }

    protected override void HandleDeathEvent()
    {
        base.HandleDeathEvent();
        movement.ToggleGravity(true);
    }
}
