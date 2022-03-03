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

    [Header("Dodge & Knockback")]

    [SerializeField] private SurfaceDetector wallCollisionDetector;
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float dodgeDistance;
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackDistance;

    [Header("General Combat Events")]

    [SerializeField] private UnityEvent readyAttackEvent;
    [SerializeField] private UnityEvent readyAttackEndEvent;
    [SerializeField] private UnityEvent hurtEvent;

    public Rigidbody2D Rigidbody2d { get => rigidbody2d; }
    public Animator Animator { get => animator; }
    public LayerMask AttackEffectLayer { get => attackEffectLayer; }

    public SurfaceDetector WallCollisionDetector { get => wallCollisionDetector; }
    public float DodgeSpeed { get => dodgeSpeed; }
    public float DodgeDistance { get => dodgeDistance; }
    public float KnockbackSpeed { get => knockbackSpeed; }
    public float KnockbackDistance { get => knockbackDistance; }

    public UnityEvent ReadyAttackEvent { get => readyAttackEvent; }
    public UnityEvent ReadyAttackEndEvent { get => readyAttackEndEvent; }
    public UnityEvent HurtEvent { get => hurtEvent; }

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
        readyAttackEvent.Invoke();
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

    public void Dodge(Vector2 direction)
    {
        CombatStateMachine.ChangeState(new DodgeState(this, direction));
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
        hurtEvent.Invoke();
    }

    public void OnHurtEnd()
    {
        if (CombatStateMachine.CurrState is HurtState)
        {
            CombatStateMachine.Exit();
        }
    }
}
