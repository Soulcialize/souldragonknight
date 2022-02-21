using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] protected GameObject escapeMenuUi;
    [SerializeField] protected PlayerInput playerInput;

    private InputAction menuAction;
    public static bool isMenuOpen = false;

    protected virtual void Awake()
    {
        menuAction = playerInput.actions["Menu"];
    }

    protected virtual void OnEnable()
    {
        if (playerInput.inputIsActive)
        {
            BindInputActionHandlers();
        }
    }

    protected virtual void OnDisable()
    {
        if (playerInput.inputIsActive)
        {
            UnbindInputActionHandlers();
        }
    }

    protected void BindInputActionHandlers()
    {
        menuAction.performed += HandleMenuInput;
    }

    protected void UnbindInputActionHandlers()
    {
        menuAction.performed += HandleMenuInput;
    }

    private void HandleMenuInput(InputAction.CallbackContext context)
    {
        if (isMenuOpen)
        {
            escapeMenuUi.SetActive(false);
        } else
        {
            escapeMenuUi.SetActive(true);
        }
        isMenuOpen = !isMenuOpen;
    }
}