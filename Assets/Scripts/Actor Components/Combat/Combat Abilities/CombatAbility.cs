using UnityEngine;

public enum CombatAbilityIdentifier
{
    ATTACK_MELEE,
    ATTACK_RANGED,
    BLOCK,
    DODGE
}

public abstract class CombatAbility : MonoBehaviour
{
    [SerializeField] private bool isEnabled = true;

    public bool IsEnabled { get => isEnabled; }

    public void Toggle(bool isEnabled)
    {
        this.isEnabled = isEnabled;
    }

    public abstract void Execute(Combat combat, params object[] parameters);

    public virtual void End(Combat combat) { }
}
