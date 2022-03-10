using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CombatStates;

public class Combat : MonoBehaviour
{
    [System.Serializable]
    private class SerializedCombatAbility
    {
        [SerializeField] private CombatAbilityIdentifier abilityIdentifier;
        [SerializeField] private CombatAbility ability;

        public CombatAbilityIdentifier AbilityIdentifier { get => abilityIdentifier; }
        public CombatAbility Ability { get => ability; }
    }

    [SerializeField] protected Rigidbody2D rigidbody2d;
    [SerializeField] protected Animator animator;
    [SerializeField] private LayerMask attackEffectLayer;
    [SerializeField] private List<SerializedCombatAbility> combatAbilities;

    [Header("Knockback")]

    [SerializeField] private SurfaceDetector wallCollisionDetector;
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackDistance;
    [SerializeField] private float postClashKnockbackRecoveryTime;

    [Header("General Combat Events")]

    [SerializeField] private UnityEvent hurtEvent;

    private Dictionary<CombatAbilityIdentifier, CombatAbility> identifierToAbilityDictionary
        = new Dictionary<CombatAbilityIdentifier, CombatAbility>();

    public Rigidbody2D Rigidbody2d { get => rigidbody2d; }
    public Animator Animator { get => animator; }
    public LayerMask AttackEffectLayer { get => attackEffectLayer; }

    public SurfaceDetector WallCollisionDetector { get => wallCollisionDetector; }
    public float KnockbackSpeed { get => knockbackSpeed; }
    public float KnockbackDistance { get => knockbackDistance; }
    public float PostClashKnockbackRecoveryTime { get => postClashKnockbackRecoveryTime; }

    public UnityEvent HurtEvent { get => hurtEvent; }

    public CombatStateMachine CombatStateMachine { get; private set; }

    protected virtual void Awake()
    {
        CombatStateMachine = new CombatStateMachine();
        foreach (SerializedCombatAbility serializedAbility in combatAbilities)
        {
            identifierToAbilityDictionary.Add(serializedAbility.AbilityIdentifier, serializedAbility.Ability);
        }
    }

    public CombatAbility GetCombatAbility(CombatAbilityIdentifier ability)
    {
        return identifierToAbilityDictionary[ability];
    }

    public void ExecuteCombatAbility(CombatAbilityIdentifier ability, params object[] parameters)
    {
        identifierToAbilityDictionary[ability].Execute(this, parameters);
    }

    public void EndCombatAbility(CombatAbilityIdentifier ability)
    {
        identifierToAbilityDictionary[ability].End(this);
    }

    public virtual void Attack() { }

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

    public void Clash(Vector2 knockbackDirection)
    {
        CombatStateMachine.ChangeState(new ClashState(this, knockbackDirection));
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
