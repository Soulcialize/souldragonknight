using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Combat : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private Collider2D collider2d;

    [Space(10)]

    [SerializeField] private List<AttackState.AttackEffectArea> attacksEffectAreas = new List<AttackState.AttackEffectArea>();
    [SerializeField] private LayerMask attackEffectLayer;
    [SerializeField] private int maxAttackNumber = 0;

    [Space(10)]

    [SerializeField] private float hurtRecoverDuration;

    [Space(10)]

    [Tooltip("Horizontal knockback force inflicted on defender when this character's attack hurts them.")]
    [SerializeField] private float hurtKnockbackForce;

    [Space(10)]

    [SerializeField] private UnityEvent attackEvent;
    [SerializeField] private UnityEvent hurtStartEvent;
    [SerializeField] private UnityEvent hurtEndEvent;

    public Rigidbody2D Rigidbody { get => rb;}
    public Animator Animator { get => anim; }
    public Collider2D Collider2d { get => collider2d; }

    public List<AttackState.AttackEffectArea> AttacksEffectAreas { get => attacksEffectAreas; }
    public LayerMask AttackEffectLayer { get => attackEffectLayer; }
    public int MaxAttackNumber { get => maxAttackNumber; }

    public float HurtRecoverDuration { get => hurtRecoverDuration; }

    public float HurtKnockbackForce { get => hurtKnockbackForce; }

    public UnityEvent HurtStartEvent { get => hurtStartEvent; }
    public UnityEvent HurtEndEvent { get => hurtEndEvent; }

    public StateMachine CombatStateMachine { get; private set; }

    void Awake()
    {
        CombatStateMachine = new CombatStateMachine();
    }

    void Update()
    {
        CombatStateMachine.Update();
    }
    
    public void Attack(bool isFacingRight)
    {
        CombatStateMachine.ChangeState(new AttackState(this, isFacingRight));
        attackEvent.Invoke();
    }
    
    public void ExecuteAttackEffect()
    {
        ((AttackState)CombatStateMachine.CurrState)?.ExecuteAttackEffect();
    }

    public void OnAttackRecovery()
    {
        ((AttackState)CombatStateMachine.CurrState)?.Recover();
    }

    public void OnAttackRecoveryEnd()
    {
        CombatStateMachine.Exit();
    }

    public void InterruptAttack()
    {
        ((AttackState)CombatStateMachine.CurrState)?.InterruptAttack();
        CombatStateMachine.Exit();
    }

    public void Stun()
    {
        CombatStateMachine.ChangeState(new StunState(this));
    }

    public void OnStunEnd()
    {
        CombatStateMachine.Exit();
    }

    public void Hurt()
    {
        CombatStateMachine.ChangeState(new HurtState(this));
    }

    public void OnHurtEnd()
    {
        ((HurtState)CombatStateMachine.CurrState)?.StartRecovery();
    }

    public void Die()
    {
        CombatStateMachine.ChangeState(new DeadState(this));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        foreach (AttackState.AttackEffectArea attackEffectArea in attacksEffectAreas)
        {
            Gizmos.DrawWireCube(attackEffectArea.LeftOrigin.position, attackEffectArea.Size);
            Gizmos.DrawWireCube(attackEffectArea.RightOrigin.position, attackEffectArea.Size);
        }
    }
}
