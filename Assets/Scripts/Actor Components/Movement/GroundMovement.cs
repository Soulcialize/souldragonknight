using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : Movement
{
    [SerializeField] protected SurfaceDetector groundDetector;

    [SerializeField] private float horizontalMoveSpeed;
    [SerializeField] private float jumpForce;

    private float horizontalMoveDirection;
    private bool isJumpRequestPosted;

    public bool IsAirborne { get; private set; }

    protected override void Start()
    {
        base.Start();
        IsAirborne = !groundDetector.IsInContact;
    }

    protected override void UpdateMovement()
    {
        if (IsAirborne)
        {
            if (rigidbody2d.velocity.y <= jumpForce / 2f)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", true);
                if (groundDetector.IsInContact)
                {
                    IsAirborne = false;
                    animator.SetBool("isFalling", false);
                    animator.SetBool("isRunning", horizontalMoveDirection != 0f);
                    FlipDirection(horizontalMoveDirection);
                }
            }
        }
        else
        {
            if (isJumpRequestPosted)
            {
                IsAirborne = true;
                isJumpRequestPosted = false;
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
                animator.SetBool("isJumping", true);
            }
            else
            {
                rigidbody2d.velocity = new Vector2(horizontalMoveDirection * horizontalMoveSpeed, rigidbody2d.velocity.y);
            }
        }
    }

    public void MoveHorizontally(float direction)
    {
        horizontalMoveDirection = direction;

        if (IsAirborne)
        {
            return;
        }

        animator.SetBool("isRunning", direction != 0f);
        FlipDirection(direction);
    }

    public void Jump()
    {
        if (!IsAirborne)
        {
            isJumpRequestPosted = true;
        }
    }
}
