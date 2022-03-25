using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CombatStates;

public class BlockAbility : CombatAbility
{
    [SerializeField] private bool canBlockProjectiles;
    [SerializeField] private float blockHitDuration;

    [Space(10)]

    [SerializeField] private UnityEvent blockHitEvent;

    public bool CanBlockProjectiles { get => canBlockProjectiles; }

    public override void Execute(Combat combat, params object[] parameters)
    {
        if (combat.ActionStateMachine.CurrState is BlockHitState blockHitState)
        {
            blockHitState.WillReturnToBlock = true;
        }
        else
        {
            BlockState.Direction blockDirection = (BlockState.Direction)parameters[0];
            combat.ActionStateMachine.ChangeState(new BlockState(combat, blockHitDuration, blockDirection, blockHitEvent));
        }
    }

    public override void End(Combat combat)
    {
        if (combat.ActionStateMachine.CurrState is BlockHitState blockHitState)
        {
            blockHitState.WillReturnToBlock = false;
        }
        else if (combat.ActionStateMachine.CurrState is BlockState)
        {
            combat.ActionStateMachine.Exit();
        }
    }
}
