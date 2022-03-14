using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

public class RangedAttackAbility : CombatAbility
{
    [SerializeField] private float maxRange;
    [SerializeField] private float readyDuration;
    [SerializeField] private float timeToLock;
    [SerializeField] private RangedProjectile projectilePrefab;
    [SerializeField] private Transform projectileOrigin;

    [Space(10)]

    [SerializeField] private RangedProjectileEvent fireRangedProjectileEvent;

    public float MaxRange { get => maxRange; }
    public RangedProjectileEvent FireRangedProjectileEvent { get => fireRangedProjectileEvent; }

    private void OnDisable()
    {
        fireRangedProjectileEvent.RemoveAllListeners();
    }

    public override void Execute(Combat combat, params object[] parameters)
    {
        if (readyDuration > 0f)
        {
            Transform target = (Transform)parameters[0];
            combat.CombatStateMachine.ChangeState(new ReadyRangedAttackState(combat, target, timeToLock, readyDuration, ReadyCallback));
        }
        else
        {
            Vector2 direction = (Vector2)parameters[0];
            combat.CombatStateMachine.ChangeState(new RangedAttackState(
                combat, projectilePrefab, projectileOrigin.position, direction, fireRangedProjectileEvent));
        }
    }

    private void ReadyCallback(Combat combat)
    {
        Vector2 direction = ((ReadyRangedAttackState)combat.CombatStateMachine.CurrState).TargetPosition - (Vector2)transform.position;
        combat.CombatStateMachine.ChangeState(new RangedAttackState(
            combat, projectilePrefab, projectileOrigin.position, direction, fireRangedProjectileEvent));
    }
}
