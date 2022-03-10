using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

public class BlockAbility : CombatAbility
{
    public override void Execute(Combat combat, params object[] parameters)
    {
        combat.CombatStateMachine.ChangeState(new BlockState(combat));
    }

    public override void End(Combat combat)
    {
        if (combat.CombatStateMachine.CurrState is BlockState)
        {
            combat.CombatStateMachine.Exit();
        }
    }
}
