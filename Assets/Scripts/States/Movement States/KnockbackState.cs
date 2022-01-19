using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackState : MovementState
{
    private readonly float force;
    private readonly bool isForceFromRight;

    public KnockbackState(Movement owner, float force, bool isForceFromRight) : base(owner)
    {
        this.force = force;
        this.isForceFromRight = isForceFromRight;
    }

    public override void Enter()
    {
        float direction = isForceFromRight ? -1 : 1;
        owner.Rigidbody.velocity = new Vector2(force * direction, owner.Rigidbody.velocity.y);
    }

    public override void Execute()
    {
        if (Mathf.Abs(owner.Rigidbody.velocity.x) <= 0.01f)
        {
            if (owner.IsTouchingGround())
            {
                owner.MovementStateMachine.ChangeState(new GroundedState(owner));
            }
            else
            {
                owner.MovementStateMachine.ChangeState(new AirborneState(owner));
            }
        }
    }

    public override void Exit()
    {

    }
}
