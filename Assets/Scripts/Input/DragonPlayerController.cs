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
    private InputAction dodgeAction;

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
    protected override void OnEnable()
    {
        base.OnEnable();

        if (photonView.IsMine)
        {
            Combat.DeathEvent.AddListener(HandleDeathEvent);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (photonView.IsMine)
        {
            Combat.DeathEvent.RemoveListener(HandleDeathEvent);
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
            combat.Attack();
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

            combat.Dodge(direction);
        }
    }
    protected void HandleDeathEvent()
    {
        movement.Rigidbody2d.gravityScale = 5;
    }
}
