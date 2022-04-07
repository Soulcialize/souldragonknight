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
    [SerializeField] private ProjectileLauncher projectileLauncher;

    [Space(10)]

    [SerializeField] private RangedProjectileEvent fireRangedProjectileEvent;

    public float MaxRange { get => maxRange; }

    public RangedProjectile ProjectilePrefab { get => projectilePrefab; }
    public Transform ProjectileOrigin { get => projectileOrigin; }

    public RangedProjectileEvent FireRangedProjectileEvent { get => fireRangedProjectileEvent; }

    private void OnDisable()
    {
        fireRangedProjectileEvent.RemoveAllListeners();
    }

    public override void Execute(Combat combat, params object[] parameters)
    {
        if (combat.Resource.CanConsume(resourceCost))
        {
            if (readyDuration > 0f)
            {
                Transform target = (Transform)parameters[0];
                combat.ActionStateMachine.ChangeState(new ReadyRangedAttackState(
                    combat, target, projectilePathDisplay, projectileLauncher,
                    timeToLock, readyDuration, ReadyCallback));
            }
            else
            {
                Vector2 direction = (Vector2)parameters[0];
                combat.ActionStateMachine.ChangeState(new RangedAttackState(
                    combat, projectilePrefab, projectileOrigin, projectileLauncher, direction,
                    combat.AttackEffectLayer, resourceCost, fireRangedProjectileEvent));
            }
        }
    }

    private void ReadyCallback(Combat combat)
    {
        Vector2 direction = ((ReadyRangedAttackState)combat.ActionStateMachine.CurrState).TargetPosition - (Vector2)transform.position;
        combat.ActionStateMachine.ChangeState(new RangedAttackState(
            combat, projectilePrefab, projectileOrigin, projectileLauncher, direction, 
            combat.AttackEffectLayer, resourceCost, fireRangedProjectileEvent));
    }
}
