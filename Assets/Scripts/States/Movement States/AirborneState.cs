using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : MovementState
{
    public AirborneState(Movement owner) : base(owner)
    {

    }

    public override void Enter()
    {
        owner.Animator.SetBool("isGrounded", false);
        owner.EnterAirborneStateEvent.Invoke();
    }

    public override void Execute()
    {
        if (owner.Rigidbody.velocity.y <= 0f)
        {
            owner.Animator.SetTrigger("StartFall");
            if (owner.IsTouchingGround())
            {
                owner.MovementStateMachine.ChangeState(new GroundedState(owner));
            }
        }
    }

    public override void Exit()
    {

    }
}
