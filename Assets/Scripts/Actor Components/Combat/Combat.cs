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

    [SerializeField] protected Collider2D collider2d;
    [SerializeField] protected Rigidbody2D rigidbody2d;
    [SerializeField] protected Animator animator;
    [SerializeField] private LayerMask attackEffectLayer;
    [SerializeField] private List<SerializedCombatAbility> combatAbilities;

    [Header("Combat Stats")]

    [SerializeField] private Health health;
    
    [Header("Knockback")]

    [SerializeField] private SurfaceDetector wallCollisionDetector;
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackDistance;

    [Header("General Combat Events")]

    [Tooltip("'actor' refers to whichever actor the OnProjectileFiredEvent is subscribed to.")]
    [SerializeField] private RangedProjectileEvent actorFiredProjectileEvent;
    [SerializeField] private UnityEvent hurtEvent;
    [SerializeField] private UnityEvent deathEvent;

    private Dictionary<CombatAbilityIdentifier, CombatAbility> identifierToAbilityDictionary
        = new Dictionary<CombatAbilityIdentifier, CombatAbility>();

    public Collider2D Collider2d { get => collider2d; }
    public Rigidbody2D Rigidbody2d { get => rigidbody2d; }
    public Animator Animator { get => animator; }
    public LayerMask AttackEffectLayer { get => attackEffectLayer; }

    public Health Health { get => health; }

    public SurfaceDetector WallCollisionDetector { get => wallCollisionDetector; }
    public float KnockbackSpeed { get => knockbackSpeed; }
    public float KnockbackDistance { get => knockbackDistance; }

    public UnityEvent HurtEvent { get => hurtEvent; }
    public UnityEvent DeathEvent { get => deathEvent; }

    public CombatStateMachine CombatStateMachine { get; private set; }

    protected virtual void Awake()
    {
        CombatStateMachine = new CombatStateMachine();
        foreach (SerializedCombatAbility serializedAbility in combatAbilities)
        {
            identifierToAbilityDictionary.Add(serializedAbility.AbilityIdentifier, serializedAbility.Ability);
        }
    }

    public void ToggleCombatAbilities(bool isEnabled)
    {
        foreach (CombatAbility ability in identifierToAbilityDictionary.Values)
        {
            ability.Toggle(isEnabled);
        }
    }

    public bool HasCombatAbility(CombatAbilityIdentifier ability)
    {
        return identifierToAbilityDictionary.ContainsKey(ability);
    }

    public CombatAbility GetCombatAbility(CombatAbilityIdentifier ability)
    {
        return identifierToAbilityDictionary[ability];
    }

    public void ExecuteCombatAbility(CombatAbilityIdentifier ability, params object[] parameters)
    {
        if (identifierToAbilityDictionary[ability].IsEnabled)
        {
            identifierToAbilityDictionary[ability].Execute(this, parameters);
        }
    }

    public void EndCombatAbility(CombatAbilityIdentifier ability)
    {
        if (identifierToAbilityDictionary[ability].IsEnabled)
        {
            identifierToAbilityDictionary[ability].End(this);
        }
    }

    public void OnProjectileFiredEvent(RangedProjectile projectile)
    {
        actorFiredProjectileEvent.Invoke(projectile);
    }

    public void Stun()
    {
        CombatStateMachine.ChangeState(new StunState(this));
    }

    public void Hurt()
    {
        if (!(CombatStateMachine.CurrState is DeathState))
        {
            health.Decrement();

            if (!health.IsZero())
            {
                CombatStateMachine.ChangeState(new HurtState(this));
                hurtEvent.Invoke();
            }
            else
            {
                CombatStateMachine.ChangeState(new DeathState(this));
                deathEvent.Invoke();
            }
        }
    }
}
