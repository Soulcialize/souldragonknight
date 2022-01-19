using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : CombatState
{
    public StunState(Combat owner) : base(owner)
    {

    }

    public override void Enter()
    {
        owner.Animator.SetTrigger("Stun");
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
