using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HurtState : CombatState
{
    private float currRecoverDuration;

    public bool IsRecovering { get; private set; }

    public HurtState(Combat owner) : base(owner)
    {

    }

    public override void Enter()
    {
        owner.Animator.SetTrigger("Hurt");
        owner.HurtStartEvent.Invoke();
    }

    public override void Execute()
    {
        if (IsRecovering)
        {
            if (currRecoverDuration >= owner.HurtRecoverDuration)
            {
                owner.CombatStateMachine.Exit();
                return;
            }

            currRecoverDuration += Time.deltaTime;
        }
    }

    public override void Exit()
    {
        owner.HurtEndEvent.Invoke();
    }

    public void StartRecovery()
    {
        currRecoverDuration = 0f;
        IsRecovering = true;
    }
}
