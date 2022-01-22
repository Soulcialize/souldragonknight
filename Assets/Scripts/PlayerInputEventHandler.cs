using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerInputEventHandler : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private InputActionMapManager actionMapManager;
    [SerializeField] private PlayerController player;

    void OnEnable()
    {
        if (photonView.IsMine)
        {
            actionMapManager.EnableInput();
        }
        else
        {
            actionMapManager.DisableInput();
            PlayerInputPhotonEventReceiver.Instance.AddPlayer(photonView.ViewID, player);
        }
    }

    private void OnDisable()
    {
        if (!photonView.IsMine)
        {
            PlayerInputPhotonEventReceiver.Instance.RemovePlayer(photonView.ViewID);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        float moveInput = context.ReadValue<float>();
        player.Move(moveInput);
        if (photonView.IsMine)
        {
            PlayerInputPhotonEventSender.Instance.SendMoveInputEvent(photonView.ViewID, moveInput);
        }
    }

    public void OnJumpGrounded(InputAction.CallbackContext context)
    {
        if (context.performed && player.Combat.CombatStateMachine.CurrState == null)
        {
            actionMapManager.SwitchInputActionMapTo("Airborne");
            player.JumpGrounded();
            if (photonView.IsMine)
            {
                PlayerInputPhotonEventSender.Instance.SendJumpInputEvent(photonView.ViewID);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && player.Movement.MovementStateMachine.CurrState is GroundedState)
        {
            player.Attack();
            if (photonView.IsMine)
            {
                PlayerInputPhotonEventSender.Instance.SendAttackInputEvent(photonView.ViewID);
            }
        }
    }
}
