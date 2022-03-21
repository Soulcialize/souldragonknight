using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerController : ActorController
{
    [SerializeField] protected PlayerInput playerInput;
    [SerializeField] protected List<string> persistentActionMaps = new List<string>();

    private InputAction menuAction;

    protected virtual void Awake()
    {
        if (!photonView.IsMine)
        {
            playerInput.DeactivateInput();
            return;
        }

        persistentActionMaps.AddRange(new List<string>() { "Ui" });
        foreach (string actionMapName in persistentActionMaps)
        {
            playerInput.actions.FindActionMap(actionMapName).Enable();
        }

        menuAction = playerInput.actions["Menu"];
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (photonView.IsMine)
        {
            BindGeneralInputActionHandlers();
            BindInputActionHandlers();

            EscapeMenu.Instance.EscapeMenuOpenEvent.AddListener(playerInput.DeactivateInput);
            EscapeMenu.Instance.EscapeMenuCloseEvent.AddListener(playerInput.ActivateInput);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (photonView.IsMine)
        {
            UnbindGeneralInputActionHandlers();
            UnbindInputActionHandlers();

            EscapeMenu.Instance.EscapeMenuOpenEvent.RemoveListener(playerInput.DeactivateInput);
            EscapeMenu.Instance.EscapeMenuCloseEvent.RemoveListener(playerInput.ActivateInput);
        }
    }

    protected virtual void BindGeneralInputActionHandlers()
    {
        menuAction.performed += HandleToggleMenuInput;
    }

    protected virtual void UnbindGeneralInputActionHandlers()
    {
        menuAction.performed -= HandleToggleMenuInput;
    }

    protected abstract void BindInputActionHandlers();

    protected abstract void UnbindInputActionHandlers();

    private void HandleToggleMenuInput(InputAction.CallbackContext context)
    {
        EscapeMenu.Instance.ToggleMenu();
    }
}
