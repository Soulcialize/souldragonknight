using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Movement movement;

    private InputAction moveAction;

    private void Awake()
    {
        moveAction = playerInput.actions["Move"];
    }

    private void OnEnable()
    {
        moveAction.performed += HandleMoveInput;
    }

    private void OnDisable()
    {
        moveAction.performed -= HandleMoveInput;
    }

    private void HandleMoveInput(InputAction.CallbackContext context)
    {
        movement.MoveHorizontally(context.ReadValue<float>());
    }
}
