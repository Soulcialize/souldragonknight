using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : ActorController
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private InputActionMapManager actionMapManager;

    private float moveInput;

    protected override void Start()
    {
        base.Start();
        if (photonView.IsMine)
        {
            actionMapManager.EnableInput();
        }
        else
        {
            actionMapManager.DisableInput();
        }
    }

    void FixedUpdate()
    {
        if (actionMapManager.IsInputActive && combat.CombatStateMachine.CurrState == null && movement.MovementStateMachine.CurrState is GroundedState groundedState)
        {
            groundedState.PostMoveRequest(moveInput);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    public void OnJumpGrounded(InputAction.CallbackContext context)
    {
        if (context.performed && combat.CombatStateMachine.CurrState == null)
        {
            actionMapManager.SwitchInputActionMapTo("Airborne");
            movement.JumpGrounded();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && movement.MovementStateMachine.CurrState is GroundedState)
        {
            combat.Attack(movement.IsFacingRight);
        }
    }

    public void OnExecuteAttackEffect()
    {
        combat.ExecuteAttackEffect();
    }
}
