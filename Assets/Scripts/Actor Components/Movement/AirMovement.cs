using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachines;
using AirMovementStates;

public class AirMovement : Movement
{
    [SerializeField] private float movementSpeed;

    [Space(10)]

    [SerializeField] private UnityEvent enterAirborneStateEvent;

    private float cachedHorizontalMoveDirection;
    private float cachedVerticalMoveDirection;

    public float MovementSpeed { get => movementSpeed; }

    public UnityEvent EnterAirborneStateEvent { get => enterAirborneStateEvent; }

    public AirMovementStateMachine MovementStateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        MovementStateMachine = new AirMovementStateMachine();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        enterAirborneStateEvent.AddListener(HandleEnterAirborneStateEvent);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        enterAirborneStateEvent.RemoveListener(HandleEnterAirborneStateEvent);
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
        cachedHorizontalMoveDirection = direction;
        if (MovementStateMachine.CurrState is AirborneState airborneState)
        {
            airborneState.UpdateHorizontalMovement(direction);
        }
    }

    public void UpdateVerticalMovement(float direction)
    {
        cachedVerticalMoveDirection = direction;
        if (MovementStateMachine.CurrState is AirborneState airborneState)
        {
            airborneState.UpdateVerticalMovement(direction);
        }
    }

    private void HandleEnterAirborneStateEvent()
    {
        AirborneState airborneState = (AirborneState)MovementStateMachine.CurrState;
        airborneState.UpdateHorizontalMovement(cachedHorizontalMoveDirection);
        airborneState.UpdateVerticalMovement(cachedVerticalMoveDirection);
    }
}
