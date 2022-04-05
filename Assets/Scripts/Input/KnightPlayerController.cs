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
    private InputAction startInteractionAction;
    private InputAction stopInteractionAction;

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
        startInteractionAction = playerInput.actions["Interact"];
        stopInteractionAction = playerInput.actions["InteractStop"];

        healthUI = FindObjectOfType<HealthUI>();
        staminaUI = FindObjectOfType<ConsumableResourceUI>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (combat.ActionStateMachine.CurrState == null)
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
            Combat.Resource.UpdateResourceEvent.AddListener(staminaUI.UpdateStaminaUI);
            Combat.Resource.RegenerateResourceEvent.AddListener(staminaUI.RegenerateStaminaUI);
            Combat.Resource.StopRegenResourceEvent.AddListener(staminaUI.StopRegenStaminaUI);
            Combat.Resource.InsufficientResourceEvent.AddListener(staminaUI.FlashStaminaWarning);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (photonView.IsMine)
        {
            Combat.Health.UpdateHealthEvent.RemoveListener(healthUI.UpdateKnightHealthUI);
            Combat.Resource.UpdateResourceEvent.RemoveListener(staminaUI.UpdateStaminaUI);
            Combat.Resource.RegenerateResourceEvent.RemoveListener(staminaUI.RegenerateStaminaUI);
            Combat.Resource.StopRegenResourceEvent.RemoveListener(staminaUI.StopRegenStaminaUI);
            Combat.Resource.InsufficientResourceEvent.RemoveListener(staminaUI.FlashStaminaWarning);
        }
    }

    protected override void BindInputActionHandlers()
    {
        moveGroundAction.performed += HandleMoveGroundInput;
        jumpAction.performed += HandleJumpInput;
        attackAction.performed += HandleAttackInput;
        blockStartAction.performed += HandleBlockStartInput;
        blockEndAction.performed += HandleBlockEndInput;
        startInteractionAction.performed += HandleStartInteractionInput;
        stopInteractionAction.performed += HandleStopInteractionInput;
    }

    protected override void UnbindInputActionHandlers()
    {
        moveGroundAction.performed -= HandleMoveGroundInput;
        jumpAction.performed -= HandleJumpInput;
        attackAction.performed -= HandleAttackInput;
        blockStartAction.performed -= HandleBlockStartInput;
        blockEndAction.performed -= HandleBlockEndInput;
        startInteractionAction.performed -= HandleStartInteractionInput;
        stopInteractionAction.performed -= HandleStopInteractionInput;
    }

    private void HandleMoveGroundInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<float>();
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        if (combat.ActionStateMachine.CurrState == null)
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

    private void HandleStartInteractionInput(InputAction.CallbackContext context)
    {
        if (combat.ActionStateMachine.CurrState == null)
        {
            Interactable nearestInteractable = interactableDetector.GetNearestInteractable();
            if (nearestInteractable != null)
            {
                Interact(nearestInteractable);
            }
            else if (Movement.MovementStateMachine.CurrState is GroundMovementStates.MountedState mountedState)
            {
                // interact button is also the dismount button
                mountedState.Dismount();
            }
        }
    }

    private void HandleStopInteractionInput(InputAction.CallbackContext context)
    {
        InterruptInteraction();
    }

    protected override void HandleDeathEvent()
    {
        base.HandleDeathEvent();

        if (Movement.MovementStateMachine.CurrState is GroundMovementStates.MountedState mountedState)
        {
            mountedState.Dismount();
        }

        PlayerManager.Instance.IncrementDeathCount();
    }

    protected override void HandleReviveStartEvent()
    {
        base.HandleReviveStartEvent();
        PlayerManager.Instance.DecrementDeathCount();
    }
}
