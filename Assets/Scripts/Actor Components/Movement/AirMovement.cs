using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using AirMovementStates;

public class AirMovement : Movement
{
    [SerializeField] private float movementSpeed;

    public float MovementSpeed { get => movementSpeed; }

    public float CachedHorizontalMoveDirection { get; private set; }
    public float CachedVerticalMoveDirection { get; private set; }
    public AirMovementStateMachine MovementStateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        MovementStateMachine = new AirMovementStateMachine();
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

    protected override void UpdateMovement()
    {
        MovementStateMachine.Update();
    }

    public void UpdateHorizontalMovement(float direction)
    {
        CachedHorizontalMoveDirection = direction;
        if (MovementStateMachine.CurrState is AirborneState airborneState)
        {
            airborneState.UpdateHorizontalMovement(direction);
        }
    }

    public void UpdateVerticalMovement(float direction)
    {
        CachedVerticalMoveDirection = direction;
        if (MovementStateMachine.CurrState is AirborneState airborneState)
        {
            airborneState.UpdateVerticalMovement(direction);
        }
    }
}
