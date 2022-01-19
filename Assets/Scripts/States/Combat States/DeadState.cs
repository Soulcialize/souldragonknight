using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : CombatState
{
    public DeadState(Combat owner) : base(owner)
    {

    }

    public override void Enter()
    {
        owner.Animator.SetBool("isDead", true);
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        owner.Animator.SetBool("isDead", false);
    }
}
