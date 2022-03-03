using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CombatStates;

public abstract class Combat : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rigidbody2d;
    [SerializeField] protected Animator animator;
    [SerializeField] private LayerMask attackEffectLayer;

    [Space(10)]

    [SerializeField] private UnityEvent readyAttackEndEvent;

    public Rigidbody2D Rigidbody2d { get => rigidbody2d; }
    public Animator Animator { get => animator; }
    public LayerMask AttackEffectLayer { get => attackEffectLayer; }

    public UnityEvent ReadyAttackEndEvent { get => readyAttackEndEvent; }

    public CombatStateMachine CombatStateMachine { get; private set; }

    protected virtual void Awake()
    {
        CombatStateMachine = new CombatStateMachine();
    }

    protected virtual void OnDisable()
    {
        readyAttackEndEvent.RemoveAllListeners();
    }

    public virtual void ReadyAttack(Transform target)
    {
        CombatStateMachine.ChangeState(new ReadyAttackState(this));
    }

    public void OnReadyAttackEnd()
    {
        readyAttackEndEvent.Invoke();
    }

    public abstract void Attack();

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

    public void Stun()
    {
        CombatStateMachine.ChangeState(new StunState(this));
    }

    public void OnStunEnd()
    {
        if (CombatStateMachine.CurrState is StunState)
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
