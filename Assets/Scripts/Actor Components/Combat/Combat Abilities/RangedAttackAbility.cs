using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;
using Photon.Pun;

public class RangedAttackAbility : CombatAbility
{
    [SerializeField] private float maxRange;
    [SerializeField] private float readyDuration;
    [SerializeField] private float timeToLock;

    [Space(10)]

    [SerializeField] private RangedProjectile projectilePrefab;
    [SerializeField] private Transform projectileOrigin;
    [SerializeField] private ProjectilePathDisplay projectilePathDisplay;

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
            combat.CombatStateMachine.ChangeState(new ReadyRangedAttackState(
                combat, target, projectilePathDisplay, timeToLock, readyDuration, ReadyCallback));
        }
        else if (combat.Resource.CanConsume(resourceCost))
        {
            combat.Resource.Consume(resourceCost);

            Vector2 direction = (Vector2)parameters[0];
            combat.CombatStateMachine.ChangeState(new RangedAttackState(
                combat, projectilePrefab, projectileOrigin, direction, combat.AttackEffectLayer, fireRangedProjectileEvent));
        }
    }

    private void ReadyCallback(Combat combat)
    {
        Vector2 direction = ((ReadyRangedAttackState)combat.CombatStateMachine.CurrState).TargetPosition - (Vector2)transform.position;
        combat.CombatStateMachine.ChangeState(new RangedAttackState(
            combat, projectilePrefab, projectileOrigin, direction, combat.AttackEffectLayer, fireRangedProjectileEvent));
    }
}
