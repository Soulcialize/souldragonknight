using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputEventHandler : MonoBehaviour
{
    [SerializeField] private InputActionMapManager actionMapManager;
    [SerializeField] private PlayerController player;

    public void OnMove(InputAction.CallbackContext context)
    {
        float moveInput = context.ReadValue<float>();
        player.Move(moveInput);
    }

    public void OnJumpGrounded(InputAction.CallbackContext context)
    {
        if (context.performed && player.Combat.CombatStateMachine.CurrState == null)
        {
            actionMapManager.SwitchInputActionMapTo("Airborne");
            player.JumpGrounded();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && player.Movement.MovementStateMachine.CurrState is GroundedState)
        {
            player.Attack();
        }
    }
}
