using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KnightPlayerController : PlayerController
{
    [SerializeField] private GroundMovement movement;

    private InputAction moveGroundAction;
    private InputAction jumpAction;

    protected override void Awake()
    {
        base.Awake();
        moveGroundAction = playerInput.actions["MoveGround"];
        jumpAction = playerInput.actions["Jump"];
    }

    protected override void BindInputActionHandlers()
    {
        moveGroundAction.performed += HandleMoveGroundInput;
        jumpAction.performed += HandleJumpInput;
    }

    protected override void UnbindInputActionHandlers()
    {
        moveGroundAction.performed -= HandleMoveGroundInput;
        jumpAction.performed -= HandleJumpInput;
    }

    private void HandleMoveGroundInput(InputAction.CallbackContext context)
    {
        movement.MoveHorizontally(context.ReadValue<float>());
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        movement.Jump();
    }
}
