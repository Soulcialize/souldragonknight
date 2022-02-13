using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Movement movement;

    private InputAction moveAction;
    private InputAction jumpAction;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            playerInput.DeactivateInput();
            return;
        }

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    private void OnEnable()
    {
        if (playerInput.inputIsActive)
        {
            moveAction.performed += HandleMoveInput;
            jumpAction.performed += HandleJumpInput;
        }
    }

    private void OnDisable()
    {
        if (playerInput.inputIsActive)
        {
            moveAction.performed -= HandleMoveInput;
            jumpAction.performed -= HandleJumpInput;
        }
    }

    private void HandleMoveInput(InputAction.CallbackContext context)
    {
        movement.MoveHorizontally(context.ReadValue<float>());
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        movement.Jump();
    }
}
