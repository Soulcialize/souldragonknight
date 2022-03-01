using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragonPlayerController : PlayerController
{
    [SerializeField] private AirMovement movement;
    [SerializeField] private Combat combat;

    private InputAction moveAirHorizontalAction;
    private InputAction moveAirVerticalAction;

    public override Movement Movement { get => movement; }
    public override Combat Combat { get => combat; }

    protected override void Awake()
    {
        base.Awake();
        moveAirHorizontalAction = playerInput.actions["MoveAirHorizontal"];
        moveAirVerticalAction = playerInput.actions["MoveAirVertical"];
    }

    protected override void BindInputActionHandlers()
    {
        moveAirHorizontalAction.performed += HandleMoveAirHorizontalInput;
        moveAirVerticalAction.performed += HandleMoveAirVerticalInput;
    }

    protected override void UnbindInputActionHandlers()
    {
        moveAirHorizontalAction.performed -= HandleMoveAirHorizontalInput;
        moveAirVerticalAction.performed -= HandleMoveAirVerticalInput;
    }

    private void HandleMoveAirHorizontalInput(InputAction.CallbackContext context)
    {
        movement.UpdateHorizontalMovement(context.ReadValue<float>());
    }

    private void HandleMoveAirVerticalInput(InputAction.CallbackContext context)
    {
        movement.UpdateVerticalMovement(context.ReadValue<float>());
    }
}
