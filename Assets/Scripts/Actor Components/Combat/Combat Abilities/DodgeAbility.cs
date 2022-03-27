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
        if (combat.Resource.CanConsume(resourceCost))
        {
            combat.Resource.Consume(resourceCost);

            Vector2 direction = (Vector2)parameters[0];
            combat.ActionStateMachine.ChangeState(new DodgeState(combat, direction, speed, distance));
        }
    }
}
