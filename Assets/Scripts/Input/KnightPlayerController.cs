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
    private InputAction blockStartAction;
    private InputAction blockEndAction;

    private HealthUI healthUI;

    private float movementInput = 0f;

    public override Movement Movement { get => movement; }

    protected override void Awake()
    {
        base.Awake();
        moveGroundAction = playerInput.actions["MoveGround"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
        blockStartAction = playerInput.actions["BlockStart"];
        blockEndAction = playerInput.actions["BlockEnd"];

        healthUI = GameObject.FindObjectOfType<HealthUI>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (combat.CombatStateMachine.CurrState == null)
        {
            movement.UpdateMovement(new Vector2(movementInput, 0f));
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (photonView.IsMine)
        {
            Combat.Health.DecrementHealthEvent.AddListener(healthUI.DecrementKnightHealthUI);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (photonView.IsMine)
        {
            Combat.Health.DecrementHealthEvent.RemoveListener(healthUI.DecrementKnightHealthUI);
        }
    }

    protected override void BindInputActionHandlers()
    {
        moveGroundAction.performed += HandleMoveGroundInput;
        jumpAction.performed += HandleJumpInput;
        attackAction.performed += HandleAttackInput;
        blockStartAction.performed += HandleBlockStartInput;
        blockEndAction.performed += HandleBlockEndInput;
    }

    protected override void UnbindInputActionHandlers()
    {
        moveGroundAction.performed -= HandleMoveGroundInput;
        jumpAction.performed -= HandleJumpInput;
        attackAction.performed -= HandleAttackInput;
        blockStartAction.performed -= HandleBlockStartInput;
        blockEndAction.performed += HandleBlockEndInput;
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
            combat.ExecuteCombatAbility(CombatAbilityIdentifier.ATTACK_MELEE);
        }
    }

    private void HandleBlockStartInput(InputAction.CallbackContext context)
    {
        if (movement.MovementStateMachine.CurrState is GroundMovementStates.GroundedState)
        {
            movement.UpdateMovement(Vector2.zero);
            combat.ExecuteCombatAbility(CombatAbilityIdentifier.BLOCK);
        }
    }

    private void HandleBlockEndInput(InputAction.CallbackContext context)
    {
        combat.EndCombatAbility(CombatAbilityIdentifier.BLOCK);
    }
}
