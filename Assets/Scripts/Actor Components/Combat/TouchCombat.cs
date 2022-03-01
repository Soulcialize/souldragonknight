using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

public class TouchCombat : Combat
{
    protected override AttackState GetNewAttackState()
    {
        return new TouchAttackState(this);
    }
}
