using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTrees;

public class DragonEnemyController : EnemyController
{
    [SerializeField] private AirMovement movement;

    public override Movement Movement { get => movement; }

    protected override BehaviorTreesManager InitializeBehaviorTreesManager()
    {
        return new BehaviorTreesManager(
            new Dictionary<BehaviorTree.Function, BehaviorTree>()
            {
                // construct behavior trees
                { BehaviorTree.Function.COMBAT, CombatTreeConstructor.ConstructMeleeCombatTree(this, movement, combat, detection) },
                { BehaviorTree.Function.IDLE, IdleTreeConstructor.ConstructIdleTree(this, movement, combat, detection) }
            },
            BehaviorTree.Function.IDLE);
    }

    protected override void HandleDeathEvent()
    {
        base.HandleDeathEvent();
        movement.ToggleGravity(true);
    }
}
