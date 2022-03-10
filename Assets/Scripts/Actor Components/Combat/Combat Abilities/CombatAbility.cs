using UnityEngine;

public enum CombatAbilityIdentifier
{
    ATTACK_MELEE,
    ATTACK_RANGED,
    BLOCK,
    DODGE,
    ATTACK_CHARGE
}

public abstract class CombatAbility : MonoBehaviour
{
    public abstract void Execute(Combat combat, params object[] parameters);

    public virtual void End(Combat combat) { }
}
