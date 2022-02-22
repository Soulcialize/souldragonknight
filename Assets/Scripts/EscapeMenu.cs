using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] protected PlayerInput playerInput;
    [SerializeField] private GameObject escapeMenuUi;

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
        UnbindInputActionHandlers();
    }

    protected void BindInputActionHandlers()
    {
        menuAction.performed += HandleMenuInput;
    }

    protected void UnbindInputActionHandlers()
    {
        menuAction.performed -= HandleMenuInput;
    }

    private void HandleMenuInput(InputAction.CallbackContext context)
    {
        if (isMenuOpen)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }     
    }

    public void OpenMenu()
    {
        escapeMenuUi.SetActive(true);
        isMenuOpen = true;
    }

    public void CloseMenu()
    {
        escapeMenuUi.SetActive(false);
        isMenuOpen = false;
    }
}
