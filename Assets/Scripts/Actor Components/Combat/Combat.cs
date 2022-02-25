using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using CombatStates;

public class Combat : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    [Space(10)]

    [SerializeField] private AttackEffectArea attackEffectArea;
    [SerializeField] private LayerMask attackEffectLayer;

    public Animator Animator { get => animator; }

    public AttackEffectArea AttackEffectArea { get => attackEffectArea; }
    public LayerMask AttackEffectLayer { get => attackEffectLayer; }

    public CombatStateMachine CombatStateMachine { get; private set; }

    private void Awake()
    {
        CombatStateMachine = new CombatStateMachine();
    }

    public void Attack(bool isFacingRight)
    {
        CombatStateMachine.ChangeState(new AttackState(this, isFacingRight));
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
