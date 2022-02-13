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
    }

    public void MoveHorizontally(float direction)
    {
        horizontalMoveDirection = direction;
    }

    public void MoveVertically(float direction)
    {
        verticalMoveDirection = direction;
    }
}
