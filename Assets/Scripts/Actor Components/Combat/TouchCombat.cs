using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

public class TouchCombat : Combat
{
    [Tooltip("Distance from the target at which to ready attack.")]
    [SerializeField] private float readyAttackDistance;

    public float ReadyAttackDistance { get => readyAttackDistance; }

    protected override AttackState GetNewAttackState()
    {
        return new TouchAttackState(this);
    }
}
