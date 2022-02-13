using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragonPlayerController : PlayerController
{
    [SerializeField] private AirMovement movement;

    private InputAction moveAirHorizontalAction;
    private InputAction moveAirVerticalAction;

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
        movement.MoveHorizontally(context.ReadValue<float>());
    }

    private void HandleMoveAirVerticalInput(InputAction.CallbackContext context)
    {
        movement.MoveVertically(context.ReadValue<float>());
    }
}
