using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private Animator animator;

    [SerializeField] private float horizontalMoveSpeed;

    private float horizontalMoveDirection;

    private void FixedUpdate()
    {
        rigidbody2d.velocity = new Vector2(horizontalMoveDirection * horizontalMoveSpeed, rigidbody2d.velocity.y);
    }

    public void MoveHorizontally(float direction)
    {
        horizontalMoveDirection = direction;

        animator.SetBool("isRunning", direction != 0f);

        // handle direction to face
        Vector3 localScale = transform.localScale;
        if (direction < 0f && localScale.x > 0f || direction > 0f && localScale.x < 0f)
        {
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        }
    }
}
