using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using AirMovementStates;

public class AirMovement : Movement
{
    [SerializeField] private float movementSpeed;

    private AirMovementStateMachine movementStateMachine;

    public float MovementSpeed { get => movementSpeed; }

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

    protected override void UpdateMovementStateMachine()
    {
        MovementStateMachine.Update();
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

    public void UpdateHorizontalMovement(float direction)
    {
        CachedMovementDirection = new Vector2(direction, CachedMovementDirection.y);
        if (MovementStateMachine.CurrState is AirborneState airborneState)
        {
            airborneState.UpdateHorizontalMovement(direction);
        }
    }

    public void UpdateVerticalMovement(float direction)
    {
        CachedMovementDirection = new Vector2(CachedMovementDirection.x, direction);
        if (MovementStateMachine.CurrState is AirborneState airborneState)
        {
            airborneState.UpdateVerticalMovement(direction);
        }
    }
}
