using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KnightPlayerController : PlayerController
{
    [SerializeField] private GroundMovement movement;
    [SerializeField] private MeleeCombat combat;

    private InputAction moveGroundAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    private float movementInput = 0f;

    public override Movement Movement { get => movement; }
    public override Combat Combat { get => combat; }

    protected override void Awake()
    {
        base.Awake();
        moveGroundAction = playerInput.actions["MoveGround"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (combat.CombatStateMachine.CurrState == null)
        {
            movement.UpdateMovement(new Vector2(movementInput, 0f));
        }
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
        movementInput = context.ReadValue<float>();
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        if (combat.CombatStateMachine.CurrState == null)
        {
            movement.Jump();
        }
    }

    private void HandleAttackInput(InputAction.CallbackContext context)
    {
        if (movement.MovementStateMachine.CurrState is GroundMovementStates.GroundedState)
        {
            movement.UpdateMovement(Vector2.zero);
            combat.Attack();
        }
    }
}
