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
    private InputAction rangedAttackAction;

    private float horizontalMovementInput = 0f;
    private float verticalMovementInput = 0f;

    public override Movement Movement { get => movement; }
    public override Combat Combat { get => combat; }

    protected override void Awake()
    {
        base.Awake();
        moveAirHorizontalAction = playerInput.actions["MoveAirHorizontal"];
        moveAirVerticalAction = playerInput.actions["MoveAirVertical"];
        rangedAttackAction = playerInput.actions["RangedAttack"];
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (combat.CombatStateMachine.CurrState == null)
        {
            movement.UpdateMovement(new Vector2(horizontalMovementInput, verticalMovementInput));
        }
    }

    protected override void BindInputActionHandlers()
    {
        moveAirHorizontalAction.performed += HandleMoveAirHorizontalInput;
        moveAirVerticalAction.performed += HandleMoveAirVerticalInput;
        rangedAttackAction.performed += HandleRangedAttackInput;
    }

    protected override void UnbindInputActionHandlers()
    {
        moveAirHorizontalAction.performed -= HandleMoveAirHorizontalInput;
        moveAirVerticalAction.performed -= HandleMoveAirVerticalInput;
        rangedAttackAction.performed -= HandleRangedAttackInput;
    }

    private void HandleMoveAirHorizontalInput(InputAction.CallbackContext context)
    {
        horizontalMovementInput = context.ReadValue<float>();
    }

    private void HandleMoveAirVerticalInput(InputAction.CallbackContext context)
    {
        verticalMovementInput = context.ReadValue<float>();
    }

    private void HandleRangedAttackInput(InputAction.CallbackContext context)
    {
        if (movement.MovementStateMachine.CurrState is AirMovementStates.AirborneState)
        {
            movement.UpdateMovement(Vector2.zero);
            combat.Attack();
        }
    }
}
