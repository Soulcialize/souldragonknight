using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public abstract class PlayerController : MonoBehaviour
{
    [SerializeField] protected PhotonView photonView;
    [SerializeField] protected PlayerInput playerInput;

    protected virtual void Awake()
    {
        if (!photonView.IsMine)
        {
            playerInput.DeactivateInput();
            return;
        }
    }

    private void Update()
    {
        if (EscapeMenu.isMenuOpen)
        {
            playerInput.DeactivateInput();
        } else
        {
            playerInput.ActivateInput();
        }
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

    protected abstract void BindInputActionHandlers();

    protected abstract void UnbindInputActionHandlers();
}
