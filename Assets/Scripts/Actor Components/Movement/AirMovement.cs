using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMovement : Movement
{
    [SerializeField] private float movementSpeed;

    private float horizontalMoveDirection;
    private float verticalMoveDirection;

    protected override void UpdateMovement()
    {
        rigidbody2d.velocity = new Vector2(horizontalMoveDirection, verticalMoveDirection).normalized * movementSpeed;
        animator.SetBool("isFlying", rigidbody2d.velocity != Vector2.zero);
    }

    public void MoveHorizontally(float direction)
    {
        horizontalMoveDirection = direction;
        FlipDirection(direction);
    }

    public void MoveVertically(float direction)
    {
        verticalMoveDirection = direction;
    }
}
