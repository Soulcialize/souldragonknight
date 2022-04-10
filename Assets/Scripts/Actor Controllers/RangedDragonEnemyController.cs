using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;
using AiBehaviorTrees;

public class RangedDragonEnemyController : EnemyController
{
    [SerializeField] private GroundMovement movement;

    public override Movement Movement { get => movement; }

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

    public void CombatTargetFiredProjectileHandler(RangedProjectile projectile)
    {
        if (!combat.HasCombatAbility(CombatAbilityIdentifier.BLOCK)
            || !((BlockAbility)combat.GetCombatAbility(CombatAbilityIdentifier.BLOCK)).CanBlockProjectiles)
        {
            // cannot block or cannot block projectiles
            return;
        }

        if (WillProjectileHitActor(projectile))
        {
            // projectile is travelling towards actor, start block
            float projectileAngle = Vector2.Angle(Vector2.up, projectile.Direction);
            BlockState.Direction blockDirection = projectileAngle >= 135f
                ? BlockState.Direction.UPWARDS
                : BlockState.Direction.HORIZONTAL;

            projectile.HitEvent.AddListener(() => combat.EndCombatAbility(CombatAbilityIdentifier.BLOCK));
            combat.ExecuteCombatAbility(CombatAbilityIdentifier.BLOCK, blockDirection);
        }
    }

    private bool WillProjectileHitActor(RangedProjectile projectile)
    {
        RaycastHit2D projectileRaycastHit = Physics2D.CircleCast(
            projectile.transform.position, projectile.GetHeight() / 2f,
            projectile.Direction, Mathf.Infinity, projectile.ActorTargetsLayer | projectile.FriendlyTargetsLayer);

        // check if projectile is going to hit this actor
        return projectileRaycastHit.collider == combat.Collider2d;
    }
}
