using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KnightPlayerController : PlayerController
{
    [SerializeField] private GroundMovement movement;

    private InputAction moveGroundAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    protected override void Awake()
    {
        base.Awake();
        moveGroundAction = playerInput.actions["MoveGround"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
    }

    protected override void BindInputActionHandlers()
    {
        moveGroundAction.performed += HandleMoveGroundInput;
        jumpAction.performed += HandleJumpInput;
        attackAction.performed += HandleAttackInput;
    }

    protected override void UnbindInputActionHandlers()
    {
        moveGroundAction.performed -= HandleMoveGroundInput;
        jumpAction.performed -= HandleJumpInput;
        attackAction.performed -= HandleAttackInput;
    }

    private void HandleMoveGroundInput(InputAction.CallbackContext context)
    {
        if (!combat.IsAttacking)
        {
            movement.UpdateHorizontalMovement(context.ReadValue<float>());
        }
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        if (!combat.IsAttacking)
        {
            movement.Jump();
        }
    }

    private void HandleAttackInput(InputAction.CallbackContext context)
    {
        if (movement.MovementStateMachine.CurrState is GroundMovementStates.GroundedState)
        {
            movement.UpdateHorizontalMovement(0f);
            combat.Attack(movement.IsFacingRight);
        }
    }
}
