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
        if (combat.CombatStateMachine.CurrState is AttackState attackState)
        {
            attackState.ExecuteAttackEffect();
        }
    }

    public void OnAttackEnd()
    {
        if (combat.CombatStateMachine.CurrState is AttackState)
        {
            combat.CombatStateMachine.Exit();
        }
    }

    public void OnStunEnd()
    {
        if (combat.CombatStateMachine.CurrState is StunState)
        {
            combat.CombatStateMachine.Exit();
        }
    }

    public void OnHurtEnd()
    {
        if (combat.CombatStateMachine.CurrState is HurtState)
        {
            combat.CombatStateMachine.Exit();
        }
    }

    public void OnDeathEndEvent()
    {
        deathAnimationEndEvent.Invoke();
    }

    public void OnReviveEnd()
    {
        if (combat.CombatStateMachine.CurrState is ReviveState)
        {
            combat.CombatStateMachine.Exit();
        }
    }
}
