using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTrees;

public class RangedKnightEnemyController : EnemyController
{
    [SerializeField] private AirMovement movement;
    [SerializeField] private Visibility projectileLauncherVisibility;

    public override Movement Movement { get => movement; }

    protected override void Start()
    {
        base.Start();

        if (photonView.IsMine)
        {
            projectileLauncherVisibility.Hide();
        }
    }

    protected override BehaviorTreesManager InitializeBehaviorTreesManager()
    {
        return new BehaviorTreesManager(
            new Dictionary<BehaviorTree.Function, BehaviorTree>()
            {
                { BehaviorTree.Function.COMBAT, CombatTreeConstructor.ConstructRangedCombatTree(this, movement, combat, detection) },
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
