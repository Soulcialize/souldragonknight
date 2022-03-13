using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

public class RangedAttackAbility : CombatAbility
{
    [SerializeField] private RangedProjectile projectilePrefab;
    [SerializeField] private Transform projectileOrigin;

    public override void Execute(Combat combat, params object[] parameters)
    {
        Vector2 direction = (Vector2)parameters[0];
        combat.CombatStateMachine.ChangeState(new RangedAttackState(
            combat,
            projectilePrefab,
            projectileOrigin.position,
            direction));
    }
}
