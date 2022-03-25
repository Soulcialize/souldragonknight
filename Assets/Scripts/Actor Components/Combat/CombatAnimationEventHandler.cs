using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CombatStates;

public class CombatAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private Combat combat;

    [Space(10)]

    [SerializeField] private UnityEvent deathAnimationEndEvent;

    public void ExecuteAttackEffect()
    {
        if (combat.ActionStateMachine.CurrState is AttackState attackState)
        {
            attackState.ExecuteAttackEffect();
        }
    }

    public void OnAttackEnd()
    {
        if (combat.ActionStateMachine.CurrState is AttackState)
        {
            combat.ActionStateMachine.Exit();
        }
    }

    public void OnStunEnd()
    {
        if (combat.ActionStateMachine.CurrState is StunState)
        {
            combat.ActionStateMachine.Exit();
        }
    }

    public void OnHurtEnd()
    {
        if (combat.ActionStateMachine.CurrState is HurtState)
        {
            combat.ActionStateMachine.Exit();
        }
    }

    public void OnDeathEndEvent()
    {
        deathAnimationEndEvent.Invoke();
    }

    public void OnReviveEnd()
    {
        if (combat.ActionStateMachine.CurrState is ReviveState)
        {
            combat.ActionStateMachine.Exit();
        }
    }
}
