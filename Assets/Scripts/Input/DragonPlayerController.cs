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
    private InputAction rangedAttackDownAction;
    private InputAction dodgeAction;

    private HealthUI healthUI;

    private float horizontalMovementInput = 0f;
    private float verticalMovementInput = 0f;

    public override Movement Movement { get => movement; }

    protected override void Awake()
    {
        base.Awake();
        moveAirHorizontalAction = playerInput.actions["MoveAirHorizontal"];
        moveAirVerticalAction = playerInput.actions["MoveAirVertical"];
        rangedAttackAction = playerInput.actions["RangedAttack"];
        rangedAttackDownAction = playerInput.actions["RangedAttackDown"];
        dodgeAction = playerInput.actions["AirDodge"];

        healthUI = GameObject.FindObjectOfType<HealthUI>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (combat.CombatStateMachine.CurrState == null)
        {
            movement.UpdateMovement(new Vector2(horizontalMovementInput, verticalMovementInput));
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        if (photonView.IsMine)
        {
            Combat.Health.DecrementHealthEvent.AddListener(healthUI.DecrementDragonHealthUI);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (photonView.IsMine)
        {
            Combat.Health.DecrementHealthEvent.RemoveListener(healthUI.DecrementDragonHealthUI);
        }
    }

    protected override void BindInputActionHandlers()
    {
        moveAirHorizontalAction.performed += HandleMoveAirHorizontalInput;
        moveAirVerticalAction.performed += HandleMoveAirVerticalInput;
        rangedAttackAction.performed += HandleRangedAttackInput;
        rangedAttackDownAction.performed += HandleRangedAttackDownInput;
        dodgeAction.performed += HandleDodgeInput;
    }

    protected override void UnbindInputActionHandlers()
    {
        moveAirHorizontalAction.performed -= HandleMoveAirHorizontalInput;
        moveAirVerticalAction.performed -= HandleMoveAirVerticalInput;
        rangedAttackAction.performed -= HandleRangedAttackInput;
        rangedAttackDownAction.performed -= HandleRangedAttackDownInput;
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

    private void HandleRangedAttackDownInput(InputAction.CallbackContext context)
    {
        if (movement.MovementStateMachine.CurrState is AirMovementStates.AirborneState)
        {
            movement.UpdateMovement(Vector2.zero);
            combat.ExecuteCombatAbility(CombatAbilityIdentifier.ATTACK_RANGED, Vector2.down);
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
    protected override void HandleDeathEvent()
    {
        base.HandleDeathEvent();
        movement.CanLandOnGround = true;
        movement.ToggleGravity(true);
    }
}
