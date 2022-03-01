using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using CombatStates;

public abstract class Combat : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    [SerializeField] private LayerMask attackEffectLayer;

    public Animator Animator { get => animator; }

    public LayerMask AttackEffectLayer { get => attackEffectLayer; }

    public CombatStateMachine CombatStateMachine { get; private set; }

    protected virtual void Awake()
    {
        CombatStateMachine = new CombatStateMachine();
    }

    protected abstract AttackState GetNewAttackState();

    public void Attack()
    {
        CombatStateMachine.ChangeState(GetNewAttackState());
    }

    public void ExecuteAttackEffect()
    {
        if (CombatStateMachine.CurrState is AttackState attackState)
        {
            attackState.ExecuteAttackEffect();
        }
    }

    public void OnAttackEnd()
    {
        if (CombatStateMachine.CurrState is AttackState)
        {
            CombatStateMachine.Exit();
        }
    }

    public void Hurt()
    {
        CombatStateMachine.ChangeState(new HurtState(this));
    }

    public void OnHurtEnd()
    {
        if (CombatStateMachine.CurrState is HurtState)
        {
            CombatStateMachine.Exit();
        }
    }
}
