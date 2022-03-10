using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;

public class DodgeAbility : CombatAbility
{
    [SerializeField] private float speed;
    [SerializeField] private float distance;

    public override void Execute(Combat combat, params object[] parameters)
    {
        Vector2 direction = (Vector2)parameters[0];
        combat.CombatStateMachine.ChangeState(new DodgeState(combat, direction, speed, distance));
    }
}
