using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private Animator animator;
    [SerializeField] private SurfaceDetector groundDetector;

    [SerializeField] private float horizontalMoveSpeed;
    [SerializeField] private float jumpForce;

    private float horizontalMoveDirection;
    private bool isJumpRequestPosted;

    private bool isAirborne;

    private void Start()
    {
        isAirborne = !groundDetector.IsInContact;
    }

    private void FixedUpdate()
    {
        if (isAirborne)
        {
            if (rigidbody2d.velocity.y <= jumpForce / 2f)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", true);
                if (groundDetector.IsInContact)
                {
                    isAirborne = false;
                    animator.SetBool("isFalling", false);
                    animator.SetBool("isRunning", horizontalMoveDirection != 0f);
                }
            }
        }
        else
        {
            if (isJumpRequestPosted)
            {
                isAirborne = true;
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

        if (isAirborne)
        {
            return;
        }

        animator.SetBool("isRunning", direction != 0f);

        // handle direction to face
        Vector3 localScale = transform.localScale;
        if (direction < 0f && localScale.x > 0f || direction > 0f && localScale.x < 0f)
        {
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        }
    }

    public void Jump()
    {
        if (!isAirborne)
        {
            isJumpRequestPosted = true;
        }
    }
}
