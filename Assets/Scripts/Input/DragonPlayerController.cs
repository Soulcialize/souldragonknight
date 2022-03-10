using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragonPlayerController : PlayerController
{
    [SerializeField] private AirMovement movement;

    private InputAction moveAirHorizontalAction;
    private InputAction moveAirVerticalAction;
    private InputAction rangedAttackAction;
    private InputAction dodgeAction;

    private float horizontalMovementInput = 0f;
    private float verticalMovementInput = 0f;

    public override Movement Movement { get => movement; }

    protected override void Awake()
    {
        base.Awake();
        moveAirHorizontalAction = playerInput.actions["MoveAirHorizontal"];
        moveAirVerticalAction = playerInput.actions["MoveAirVertical"];
        rangedAttackAction = playerInput.actions["RangedAttack"];
        dodgeAction = playerInput.actions["AirDodge"];
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
        dodgeAction.performed += HandleDodgeInput;
    }

    protected override void UnbindInputActionHandlers()
    {
        moveAirHorizontalAction.performed -= HandleMoveAirHorizontalInput;
        moveAirVerticalAction.performed -= HandleMoveAirVerticalInput;
        rangedAttackAction.performed -= HandleRangedAttackInput;
        dodgeAction.performed -= HandleDodgeInput;
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
            Vector2 direction = movement.IsFacingRight ? Vector2.right : Vector2.left;
            combat.ExecuteCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED, direction);
        }
    }

    private void HandleDodgeInput(InputAction.CallbackContext context)
    {
        if (movement.MovementStateMachine.CurrState is AirMovementStates.AirborneState)
        {
            movement.UpdateMovement(Vector2.zero);

            Vector2 direction = new Vector2(horizontalMovementInput, verticalMovementInput);
            if (direction == Vector2.zero)
            {
                // if not moving, dodge upwards
                direction.y = 1f;
            }

            combat.ExecuteCombatAbility(CombatAbilityIdentifier.DODGE, direction);
        }
    }
}
