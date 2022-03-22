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
    private InputAction interactAction;

    private HealthUI healthUI;
    private ConsumableResourceUI staminaUI;

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
        interactAction = playerInput.actions["Interact"];

        healthUI = FindObjectOfType<HealthUI>();
        staminaUI = FindObjectOfType<ConsumableResourceUI>();
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
            Combat.Health.UpdateHealthEvent.AddListener(healthUI.UpdateKnightHealthUI);
            Combat.Resource.ConsumeResourceEvent.AddListener(staminaUI.UpdateStaminaUI);
            Combat.Resource.RegenerateResourceEvent.AddListener(staminaUI.RegenerateStaminaUI);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (photonView.IsMine)
        {
            Combat.Health.UpdateHealthEvent.RemoveListener(healthUI.UpdateKnightHealthUI);
        }
    }

    protected override void BindInputActionHandlers()
    {
        moveGroundAction.performed += HandleMoveGroundInput;
        jumpAction.performed += HandleJumpInput;
        attackAction.performed += HandleAttackInput;
        blockStartAction.performed += HandleBlockStartInput;
        blockEndAction.performed += HandleBlockEndInput;
        interactAction.performed += HandleInteractInput;
    }

    protected override void UnbindInputActionHandlers()
    {
        moveGroundAction.performed -= HandleMoveGroundInput;
        jumpAction.performed -= HandleJumpInput;
        attackAction.performed -= HandleAttackInput;
        blockStartAction.performed -= HandleBlockStartInput;
        blockEndAction.performed -= HandleBlockEndInput;
        interactAction.performed -= HandleInteractInput;
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
        if (movement.MovementStateMachine.CurrState is GroundMovementStates.GroundedState
            || movement.MovementStateMachine.CurrState is GroundMovementStates.MountedState)
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
            combat.ExecuteCombatAbility(CombatAbilityIdentifier.BLOCK, CombatStates.BlockState.Direction.HORIZONTAL);
        }
    }

    private void HandleBlockEndInput(InputAction.CallbackContext context)
    {
        combat.EndCombatAbility(CombatAbilityIdentifier.BLOCK);
    }

    private void HandleInteractInput(InputAction.CallbackContext context)
    {
        if (combat.CombatStateMachine.CurrState == null)
        {
            Interactable nearestInteractable = interactableDetector.GetNearestInteractable();
            if (nearestInteractable != null)
            {
                Interact(nearestInteractable);
            }
        }
    }

    protected override void HandleDeathEvent()
    {
        base.HandleDeathEvent();
        movement.Dismount();
    }
}
