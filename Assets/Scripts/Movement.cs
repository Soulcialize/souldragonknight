using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private float horizontalMoveSpeed;

    private float horizontalMoveDirection;

    private void FixedUpdate()
    {
        rigidbody2d.velocity = new Vector2(horizontalMoveDirection * horizontalMoveSpeed, rigidbody2d.velocity.y);
    }

    public void MoveHorizontally(float direction)
    {
        horizontalMoveDirection = direction;
    }
}
