using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachines;
using GroundMovementStates;

public class GroundMovement : Movement
{
    [SerializeField] protected SurfaceDetector groundDetector;
    [SerializeField] private float horizontalMoveSpeed;
    [SerializeField] private float jumpForce;

    [Space(10)]

    [SerializeField] private UnityEvent enterGroundedStateEvent;

    private float cachedHorizontalMovementDirection;

    public SurfaceDetector GroundDetector { get => groundDetector; }
    public float HorizontalMoveSpeed { get => horizontalMoveSpeed; }
    public float JumpForce { get => jumpForce; }

    public UnityEvent EnterGroundedStateEvent { get => enterGroundedStateEvent; }

    public GroundMovementStateMachine MovementStateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        MovementStateMachine = new GroundMovementStateMachine();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        enterGroundedStateEvent.AddListener(HandleEnterGroundedStateEvent);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        enterGroundedStateEvent.RemoveListener(HandleEnterGroundedStateEvent);
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
        cachedHorizontalMovementDirection = direction;
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

    private void HandleEnterGroundedStateEvent()
    {
        ((GroundedState)MovementStateMachine.CurrState).UpdateHorizontalMovement(cachedHorizontalMovementDirection);
    }
}
