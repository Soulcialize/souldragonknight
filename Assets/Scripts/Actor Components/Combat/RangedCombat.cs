using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

public class RangedCombat : Combat
{
    [Header("Ranged Combat")]

    [SerializeField] private RangedProjectile projectilePrefab;
    [SerializeField] private Transform projectileOrigin;

    public override void Attack()
    {
        CombatStateMachine.ChangeState(new RangedAttackState(
            this,
            projectilePrefab,
            projectileOrigin.position,
            transform.localScale.x > 0f ? Vector2.right : Vector2.left));
    }
}
