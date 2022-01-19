using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : MovementState
{
    private bool isMoveRequestPending = false;
    private float moveRequestDirection = 0f;

    private bool isJumpRequestPending = false;

    public GroundedState(Movement owner) : base(owner)
    {

    }

    public override void Enter()
    {
        owner.Animator.SetBool("isGrounded", true);
        owner.EnterGroundedStateEvent.Invoke();
    }

    public override void Execute()
    {
        if (!owner.IsTouchingGround())
        {
            owner.MovementStateMachine.ChangeState(new AirborneState(owner));
        }
        else if (isJumpRequestPending)
        {
            isJumpRequestPending = false;
            Jump();
        }
        else if (isMoveRequestPending)
        {
            isMoveRequestPending = false;
            Move(moveRequestDirection);
        }
    }

    public override void Exit()
    {
        owner.Rigidbody.gravityScale = owner.DefaultGravityScale;
        owner.Animator.SetBool("isMoving", false);
    }

    public void PostMoveRequest(float direction)
    {
        isMoveRequestPending = true;
        moveRequestDirection = direction;
    }

    public void PostJumpRequest()
    {
        isJumpRequestPending = true;
    }

    private void Move(float direction)
    {
        if (direction != -1f && direction != 0f && direction != 1f)
        {
            throw new System.ArgumentException($"Movement direction value cannot be {direction}");
        }

        if (owner.IsFacingRight && direction < 0f || !owner.IsFacingRight && direction > 0f)
        {
            owner.Flip();
        }

        // TODO: slide down slope if absolute value of angle exceeds a certain value
        RaycastHit2D raycastDown = Physics2D.Raycast(owner.transform.position, Vector2.down, Mathf.Infinity, owner.GroundLayer);
        float groundAngle = Vector2.SignedAngle(Vector2.up, raycastDown.normal);

        float horizontalVelocity = direction;
        float verticalVelocity = 0f;

        if (direction != 0f && groundAngle != 0f)
        {
            horizontalVelocity *= Mathf.Cos(groundAngle * Mathf.Deg2Rad);
            verticalVelocity += Mathf.Sin(groundAngle * Mathf.Deg2Rad);
            if (!owner.IsFacingRight)
            {
                verticalVelocity *= -1f;
            }
        }

        owner.Rigidbody.gravityScale = groundAngle == 0f ? owner.DefaultGravityScale : 0f;
        owner.Rigidbody.velocity = new Vector2(horizontalVelocity, verticalVelocity).normalized * owner.MoveSpeed;
        if (owner.IsCrouched)
        {
            owner.Rigidbody.velocity *= owner.CrouchSpeedModifier;
        }

        owner.Animator.SetBool("isMoving", direction != 0f);
    }

    private void Jump()
    {
        owner.Rigidbody.velocity = new Vector2(owner.Rigidbody.velocity.x, owner.JumpForce);
        owner.Animator.SetTrigger("Jump");

        owner.MovementStateMachine.ChangeState(new AirborneState(owner));
    }
}
