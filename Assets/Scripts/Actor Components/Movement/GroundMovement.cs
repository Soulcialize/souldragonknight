using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using GroundMovementStates;

public class GroundMovement : Movement
{
    [SerializeField] protected SurfaceDetector groundDetector;
    [SerializeField] private float horizontalMoveSpeed;
    [SerializeField] private float jumpForce;

    public SurfaceDetector GroundDetector { get => groundDetector; }
    public float HorizontalMoveSpeed { get => horizontalMoveSpeed; }
    public float JumpForce { get => jumpForce; }

    public float CachedHorizontalMovementDirection { get; private set; }
    public GroundMovementStateMachine MovementStateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        MovementStateMachine = new GroundMovementStateMachine();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    {
        base.Start();
        if (groundDetector.IsInContact)
        {
            MovementStateMachine.ChangeState(new GroundedState(this));
        }
        else
        {
            MovementStateMachine.ChangeState(new AirborneState(this));
        }
    }

    protected override void UpdateMovement()
    {
        MovementStateMachine.Update();
    }

    public void UpdateHorizontalMovement(float direction)
    {
        CachedHorizontalMovementDirection = direction;
        if (MovementStateMachine.CurrState is GroundedState groundedState)
        {
            groundedState.UpdateHorizontalMovement(direction);
        }
    }

    public void Jump()
    {
        if (MovementStateMachine.CurrState is GroundedState groundedState)
        {
            groundedState.PostJumpRequest();
        }
    }
}
