using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using AirMovementStates;

public class AirMovement : Movement
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool canLandOnGround;

    private AirMovementStateMachine movementStateMachine;

    public float MovementSpeed { get => movementSpeed; }
    public bool CanLandOnGround { get => canLandOnGround; set => canLandOnGround = value; }

    public override MovementStateMachine MovementStateMachine { get => movementStateMachine; }

    protected override void Awake()
    {
        base.Awake();
        movementStateMachine = new AirMovementStateMachine();
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
        MovementStateMachine.ChangeState(new AirborneState(this));
    }

    public override void UpdateMovement(Vector2 direction)
    {
        CachedMovementDirection = direction;
        if (MovementStateMachine.CurrState is AirborneState airborneState)
        {
            airborneState.UpdateHorizontalMovement(direction.x);
            airborneState.UpdateVerticalMovement(direction.y);
        }
    }

    public void ToggleGravity(bool isEnabled)
    {
        rigidbody2d.gravityScale = isEnabled ? 5f : 0f;
        MovementStateMachine.ChangeState(new FallingState(this));
    }
}
